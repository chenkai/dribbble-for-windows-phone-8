using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using MoCommon.EntityModel.Social;

namespace MoCommon.Social
{
    interface IShareable
    {
         AuthorizeConfig GetAuthorizeConfig(PlatformType platformType);
         string GetAuthorizeCode(PlatformType platformType,string responseType);
         void GetRequestAccessToken(PlatformType platformType,string grantType,string code);
         void SendPureTextContent(PlatformType platformType, string content);
         void SendTextWithPicContent(PlatformType platformType, string content, Stream picFileStream);
         bool ValidateAccessTokenIsValid(PlatformType platformType);
    }
}
