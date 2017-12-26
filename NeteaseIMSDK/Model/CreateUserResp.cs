using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NeteaseIMSDK.Model
{
    [Serializable]
    public class CreateUserResp : RespBase
    {
        [JsonProperty("info")]
        public NIMUser Info { get; set; }

        public string Msg
        {
            get
            {
                ErrorInfo ei = errCodeInfo.FirstOrDefault(p => p.code == this.Code);
                if (ei == null) return "未知错误";
                return ei.name;
            }
        }

        private static List<Model.ErrorInfo> errCodeInfo = new List<Model.ErrorInfo>(){
            new Model.ErrorInfo(){code=200,name="操作成功"},
            new Model.ErrorInfo(){code=201,name="客户端版本不对，需升级sdk"},
            new Model.ErrorInfo(){code=301,name="被封禁"},
            new Model.ErrorInfo(){code=302,name="用户名或密码错误"},
            new Model.ErrorInfo(){code=315,name="IP限制"},
            new Model.ErrorInfo(){code=403,name="非法操作或没有权限"},
            new Model.ErrorInfo(){code=404,name="对象不存在"},
            new Model.ErrorInfo(){code=405,name="参数长度过长"},
            new Model.ErrorInfo(){code=406,name="对象只读"},
            new Model.ErrorInfo(){code=408,name="客户端请求超时"},
            new Model.ErrorInfo(){code=413,name="验证失败(短信服务)"},
            new Model.ErrorInfo(){code=414,name="参数错误"},
            new Model.ErrorInfo(){code=415,name="客户端网络问题"},
            new Model.ErrorInfo(){code=416,name="频率控制"},
            new Model.ErrorInfo(){code=417,name="重复操作"},
            new Model.ErrorInfo(){code=418,name="通道不可用(短信服务)"},
            new Model.ErrorInfo(){code=419,name="数量超过上限"},
            new Model.ErrorInfo(){code=422,name="账号被禁用"},
            new Model.ErrorInfo(){code=431,name="HTTP重复请求"},
            new Model.ErrorInfo(){code=500,name="服务器内部错误"},
            new Model.ErrorInfo(){code=503,name="服务器繁忙"},
            new Model.ErrorInfo(){code=514,name="服务不可用"},
            new Model.ErrorInfo(){code=509,name="无效协议"},
            new Model.ErrorInfo(){code=998,name="解包错误"},
            new Model.ErrorInfo(){code=999,name="打包错误"},
            new Model.ErrorInfo(){code=801,name="群人数达到上限"},
            new Model.ErrorInfo(){code=802,name="没有权限"},
            new Model.ErrorInfo(){code=803,name="群不存在"},
            new Model.ErrorInfo(){code=804,name="用户不在群"},
            new Model.ErrorInfo(){code=805,name="群类型不匹配"},
            new Model.ErrorInfo(){code=806,name="创建群数量达到限制"},
            new Model.ErrorInfo(){code=807,name="群成员状态错误"},
            new Model.ErrorInfo(){code=808,name="申请成功"},
            new Model.ErrorInfo(){code=809,name="已经在群内"},
            new Model.ErrorInfo(){code=810,name="邀请成功"},
            new Model.ErrorInfo(){code=9102,name="通道失效"},
            new Model.ErrorInfo(){code=9103,name="已经在他端对这个呼叫响应过了"},
            new Model.ErrorInfo(){code=11001,name="通话不可达，对方离线状态"},
            new Model.ErrorInfo(){code=13001,name="IM主连接状态异常"},
            new Model.ErrorInfo(){code=13002,name="聊天室状态异常"},
            new Model.ErrorInfo(){code=13003,name="账号在黑名单中,不允许进入聊天室"},
            new Model.ErrorInfo(){code=13004,name="在禁言列表中,不允许发言"},
            new Model.ErrorInfo(){code=10431,name="输入email不是邮箱"},
            new Model.ErrorInfo(){code=10432,name="输入mobile不是手机号码"},
            new Model.ErrorInfo(){code=10433,name="注册输入的两次密码不相同"},
            new Model.ErrorInfo(){code=10434,name="企业不存在"},
            new Model.ErrorInfo(){code=10435,name="登陆密码或帐号不对"},
            new Model.ErrorInfo(){code=10436,name="app不存在"},
            new Model.ErrorInfo(){code=10437,name="email已注册"},
            new Model.ErrorInfo(){code=10438,name="手机号已注册"},
            new Model.ErrorInfo(){code=10441,name="app名字已经存在"}
      };
    }
}
