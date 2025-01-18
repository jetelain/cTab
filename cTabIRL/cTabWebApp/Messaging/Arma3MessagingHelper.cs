using System;
using System.Collections.Generic;
using System.Linq;
using Arma3TacMapLibrary.Arma3;

#nullable enable

namespace cTabWebApp.Messaging
{
    public static class Arma3MessagingHelper
    {
        public static MessageTemplate TemplateFromArma(string entry)
        {
            return TemplateFromArma(ArmaSerializer.ParseMixedArray(entry));
        }

        public static MessageTemplate TemplateFromArma(object[] arma3Data)
        {
            return new MessageTemplate
            {
                Uid = arma3Data[0] as string,
                Type = (MessageTemplateType)Convert.ToInt32(arma3Data[1]),
                Title = arma3Data[2] as string,
                ShortTitle = arma3Data[3] as string,
                Href = arma3Data[4] as string,
                JsonHref = (arma3Data[4] as string) + "&format=json",
                Lines = ((object?[])arma3Data[5]).Cast<object[]>().Select(l => new MessageLineTemplate
                {
                    Title = l[0] as string,
                    Description = l[1] as string,
                    Fields = ((object?[])l[2]).Cast<object[]>().Select(f => new MessageFieldTemplate
                    {
                        Title = f[0] as string,
                        Description = f[1] as string,
                        Type = (MessageFieldType)Convert.ToInt32(f[2])
                    }).ToList()
                }).ToList()
            };
        }

        internal static Message MessageFromArma(string entry)
        {
            return MessageFromArma(ArmaSerializer.ParseMixedArray(entry));
        }

        internal static Message MessageFromArma(object[] data)
        {
            var message = new Message()
            {
                Title = (string)data[0],
                Body = (string)data[1],
                State = Convert.ToInt32(data[2]),
                Id = (string)data[3],
            };
            if (data.Length > 4)
            {
                message.Type = (MessageTemplateType)Convert.ToInt32(data[4]);
            }
            if (data.Length > 5)
            {
                message.Attachments = ((object[])data[5])?.Cast<object[]>().Select(AttachmentFromArma).ToList();
            }
            return message;
        }

        private static MessageAttachment AttachmentFromArma(object[] data)
        {
            var attachment = new MessageAttachment()
            {
                Type = (MessageAttachmentType)Convert.ToInt32(data[0]),
                Label = (string)data[1],
            };
            if (attachment.Type == MessageAttachmentType.Grid || attachment.Type == MessageAttachmentType.Marker)
            {
                attachment.MarkerPosition = ((object[])data[2])?.Cast<double>().ToArray();
                attachment.Position = ((object[])data[3])?.Cast<double>().ToArray();
                attachment.PositionPrecision = ((object[])data[4])?.Cast<double>().ToArray();
            }
            return attachment;
        }

        public static string ToArmaSimpleArrayString(WebSendMessageMessage message)
        {
            return ArmaSerializer.ToSimpleArrayString(new object[]{
                message.To ?? string.Empty,
                message.Body ?? string.Empty,
                new object[] {
                        message.Title ?? string.Empty,
                        string.Empty,
                        (int)message.Type,
                        ToArma(message.Attachments)
                    }
                });
        }

        public static object[] ToArma(List<MessageAttachment>? attachments)
        {
            return attachments?.Select(ToArma)?.ToArray() ?? Array.Empty<object>();
        }

        public static object[] ToArma(MessageAttachment attachment)
        {
            var armaData = new List<object>();
            armaData.Add((int)attachment.Type);
            armaData.Add(attachment.Label ?? string.Empty);
            if (attachment.Type == MessageAttachmentType.Grid || attachment.Type == MessageAttachmentType.Marker)
            {
                armaData.Add(attachment.MarkerPosition ?? Array.Empty<double>());
                armaData.Add(attachment.Position ?? Array.Empty<double>());
                armaData.Add(attachment.PositionPrecision ?? Array.Empty<double>());
            }
            return armaData.ToArray();
        }
    }
}
