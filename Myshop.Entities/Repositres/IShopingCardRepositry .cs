﻿using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositres
{
    public interface IShopingCardRepositry:IGenericRepositry<ShopingCard>
    {
        int increaseCount(ShopingCard shopingCard,int count);
        int DecreaseCount(ShopingCard shopingCard,int count);
    }
}
