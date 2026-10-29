using System.Net;

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
        if (address.SubjectId is not EmailAddress to)
        {
            throw new ArgumentException("Resend only dispatches OTPs to email addresses.", nameof(address));
        }

        EmailMessage message = new()
        {
            From = _options.From,
            To = to.Value,
            Template = new EmailMessageTemplate
            {
                TemplateId = _options.TemplateId,
                Variables = new Dictionary<string, object>
                {
                    ["CODE"] = string.Join('-', otp.ToTextGroups()),
                    ["EXPIRES_IN_MINUTES"] = (int)expiresAfter.TotalMinutes,
                },
            },
        };

        try
        {
            ResendResponse<Guid> response = await resend.EmailSendAsync
            (
                idempotencyKey: Guid.NewGuid().ToString(),
                email: message,
                cancellationToken: ct
            );

            LogOtpEmailSent(logger, response.Content);
        }
        catch (ResendException exception)
        {
            LogOtpEmailFailed
            (
                logger: logger,
                exception: exception,
                errorType: exception.ErrorType,
                statusCode: exception.StatusCode
            );

            throw;
        }
    }

    [LoggerMessage(LogLevel.Information, "Resend accepted OTP email {EmailId}")]
    private static partial void LogOtpEmailSent(ILogger logger, Guid emailId);

    [LoggerMessage(LogLevel.Error, "Resend failed to send the OTP email with {ErrorType} (HTTP {StatusCode})")]
    private static partial void LogOtpEmailFailed
    (
        ILogger logger,
        Exception exception,
        ErrorType errorType,
        HttpStatusCode? statusCode
    );
}
