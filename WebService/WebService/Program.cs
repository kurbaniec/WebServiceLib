﻿using System;
using WebService_Lib;

namespace WebService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SimpleWebService();
            service.Start();
        }
    }
}