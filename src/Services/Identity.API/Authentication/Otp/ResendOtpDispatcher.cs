using Duende.UserManagement.Authentication.Otp;

using Microsoft.Extensions.Options;

using Resend;

using EmailAddress = Duende.UserManagement.EmailAddress;

namespace Identity.API.Authentication.Otp;

internal sealed partial class ResendOtpDispatcher
(
    IResend resend,
    IOptions<ResendOtpOptions> options,
    ILogger<ResendOtpDispatcher> logger
) : IOtpDispatcher
{
    private readonly ResendOtpOptions _options = options.Value;

    public bool CanDispatch(OtpAddress address) =>
        address.Channel == OtpChannel.Email && address.SubjectId is EmailAddress;

    public async Task DispatchAsync
    (
        OtpAddress address,
        PlainTextOtp otp,
        TimeSpan expiresAfter,
        CancellationToken ct
    )
    {
        if (!CanDispatch(address))
        {
            throw new ArgumentException("Only email OTP addresses are supported.", nameof(address));
        }

        EmailAddress to = (EmailAddress)address.SubjectId;

        EmailMessage message = new()
        {
            From = _options.From,
            To = to.Value,
            Template = new EmailMessageTemplate()
            {
                TemplateId = _options.TemplateId,
                Variables = new Dictionary<string, object>
                {
                    ["OTP_CODE"] = otp.Value,
                    ["EXPIRES_IN_MINUTES"] = (int)expiresAfter.TotalMinutes,
                },
            }
        };

        try
        {
            await resend.EmailSendAsync
            (
                idempotencyKey: IdempotencyKey.New(),
                email: message,
                cancellationToken: ct
            );
        }
        catch (Exception ex)
        {
            LogSendFailed(logger, ex);

            throw;
        }

        LogSent(logger);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "OTP email sent via Resend")]
    private static partial void LogSent(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to send OTP email via Resend")]
    private static partial void LogSendFailed(ILogger logger, Exception exception);
}
