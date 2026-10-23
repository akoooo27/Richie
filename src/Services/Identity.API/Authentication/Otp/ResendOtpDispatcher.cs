using System.Net;

using Duende.UserManagement.Authentication.Otp;

using Microsoft.Extensions.Options;

using Resend;

using EmailAddress = Duende.UserManagement.EmailAddress;

namespace Identity.API.Authentication.Otp;

internal sealed partial class ResendOtpDispatcher
(
    IResend resend,
    IOptions<ResendOtpDispatcherOptions> options,
    ILogger<ResendOtpDispatcher> logger
) : IOtpDispatcher
{
    private readonly ResendOtpDispatcherOptions _options = options.Value;

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

        EmailAddress emailAddress = (EmailAddress)address.SubjectId;

        EmailMessage message = new()
        {
            From = _options.From,
            To = emailAddress.Value,
            Template = new EmailMessageTemplate
            {
                TemplateId = _options.TemplateId,
                Variables = new Dictionary<string, object>
                {
                    ["OTP_CODE"] = string.Join('-', otp.ToTextGroups()),
                    ["EXPIRES_IN_MINUTES"] = (int)expiresAfter.TotalMinutes
                }
            }
        };

        try
        {
            ResendResponse<Guid> response = await resend.EmailSendAsync
            (
                idempotencyKey: IdempotencyKey.New(),
                email: message,
                cancellationToken: ct
            );

            LogOtpEmailSent(response.Content);
        }
        catch (ResendException exception)
        {
            LogOtpEmailFailed
            (
                exception: exception,
                errorType: exception.ErrorType,
                statusCode: exception.StatusCode
            );

            throw;
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "OTP email {EmailId} accepted by Resend")]
    private partial void LogOtpEmailSent(Guid emailId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Resend failed to send OTP email: {ErrorType} (HTTP {StatusCode})")]
    private partial void LogOtpEmailFailed(Exception exception, ErrorType errorType, HttpStatusCode? statusCode);
}
