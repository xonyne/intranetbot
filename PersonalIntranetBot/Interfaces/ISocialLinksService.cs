using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalIntranetBot.Interfaces
{
    public interface ISocialLinksService
    {
        string GetLinkedInAccountURLFromNameAndCompany(string name, string company);
        string GetTwitterAccountURLFromNameAndCompany(string name, string company);
        string GetXingAccountURLFromNameAndCompany(string name, string company);
    }
}
