﻿using System.Collections.Generic;

namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }

        public List<ShoppingCartItems> Items { get; set; }

        public decimal TotalPrice
        { 
            get
            {
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                    
                }
                return totalPrice;
            } 
            
            
        }
        public ShoppingCart()
        {
              Items  = new List<ShoppingCartItems>();
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
            Items = new List<ShoppingCartItems>();
            
        }

    }
}
