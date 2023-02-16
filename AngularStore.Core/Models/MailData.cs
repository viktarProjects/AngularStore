﻿namespace AngularStore.Core.Models
{
    public class MailData
    {
        // Receiver
        public List<string> To { get; }

        // Sender
        public string? From { get; }

        public string? DisplayName { get; }

        // Content
        public string Subject { get; }

        public string? Body { get; }

        public MailData(List<string> to, string subject, string? body = null, string? from = null, string? displayName = null, string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null)
        {
            // Receiver
            To = to;

            // Sender
            From = from;
            DisplayName = displayName;

            // Content
            Subject = subject;
            Body = body;
        }
    }
}
