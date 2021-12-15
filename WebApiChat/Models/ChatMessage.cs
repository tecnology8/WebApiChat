﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiChat.Models
{
    public class ChatMessage
    {
        public string SenderName { get; set; }
        public string Text { get; set; }
        public DateTime SendAt { get; set; }
    }
}