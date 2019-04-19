﻿using System;
namespace GoodREST.Core.Test.DataModel.DTO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }

    }
}