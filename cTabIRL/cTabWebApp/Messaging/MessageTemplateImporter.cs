using System;
using System.Linq;

namespace cTabWebApp.Messaging
{
    public static class MessageTemplateImporter
    {
        public static MessageTemplate FromArma3(object[] arma3Data)
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
    }
}
