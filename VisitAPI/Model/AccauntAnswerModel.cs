using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitAPI.Model
{
    public class AccauntAnswerModel
    {
        private string _msg = string.Empty;
        public bool IsSuccessfully { get; set; }

        public string Message
        {
            get => _msg;
            set
            {   
                if(!string.IsNullOrEmpty(value))
                {
                    string[] msg = value.Split(']');
                    // Парсим в порядке приоритета до первой строки

                    for (int i = 0; i < msg.Length; i++)
                    {
                        try
                        {
                            if (msg[i].IndexOf("non_field_errors") != -1)
                            {
                                _msg = ParserMsg(msg[i]);
                                return;
                            }

                            if (msg[i].IndexOf("email") != -1)
                            {
                                _msg = ParserMsg(msg[i]);
                                return;
                            }

                            if (msg[i].IndexOf("password1") != -1)
                            {
                                _msg = ParserMsg(msg[i]);
                                return;
                            }

                            if (msg[i].IndexOf("tos") != -1)
                            {
                                _msg = ParserMsg(msg[i]);
                                return;
                            }
                        }
                        catch(Exception el)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private string ParserMsg(string msg)
        {
            return msg.Substring(msg.IndexOf("[") + 1, msg.LastIndexOf('"') - msg.IndexOf("[") - 1).Trim('"');
        }

        public AccauntAnswerModel(bool IsSuccessfully, string Message)
        {
            this.IsSuccessfully = IsSuccessfully;
            this.Message = Message;
        }
    }
}
