﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Interfaces
{
    public interface IGoogleMapsService
    {
        String GetGoogleMapsURL(String destination);
    }
}
