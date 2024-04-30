using BusinessEntity;
using BusinessEntity.Other;
using BusinessInterface;
using BusinessService;
using CommonUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace BgManageWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IFunctionsService _iFunctionsService;
        public CommonController(IFunctionsService iFunctionsService)
        {
            _iFunctionsService = iFunctionsService;
        }

        /// <summary>
        /// 获取函数列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> GetFunctions(string lang="")
        {
            var list = _iFunctionsService.GetFunctionsList(lang); 
            return await ApiResult.Success(list);
        }


        #region 导入函数的基础数据，只用一次哦，先改成private，用的时候在暴露出去
        [HttpGet]
        private   async Task<ApiResultDto> TempJsonSaveDb()
        {
            var directorypath = Directory.GetCurrentDirectory();
            string strFileName = directorypath + "\\tempJson\\fs1.json";
            string jsonData = GetJsonFile(strFileName);
            
            //Console.WriteLine(jsonData);
            //反序列化Json字符串内容为对象
            //List<TempJsonFuncDto> jsondata = JsonConvert.DeserializeObject<List<TempJsonFuncDto>>(jsonData!)!;  
            //// 将 JSON 字符串解析为 JsonDocument
            //JsonDocument doc = JsonDocument.Parse(jsonData); 
            //// 遍历 JSON 树
            //VisitJsonElement(doc.RootElement);


            JsonDocument doc = JsonDocument.Parse(jsonData);
            JsonElement root = doc.RootElement;
            //List<FunctionsDto> listDto=new List<FunctionsDto> ();
            int i = 3;
            foreach (JsonElement element in root.EnumerateArray())
            {
                //string name = personElement.GetProperty("name").GetString();
                //int age = personElement.GetProperty("age").GetInt32();
                //List<string> pets = new List<string>(); 

                //JsonSerializerOptions options = new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true,
                //}; 
                //var obj=System.Text.Json.JsonSerializer.Deserialize<TempJsonFuncDto>(elment.GetRawText(), options);  
                //foreach (JsonElement childElement in elment.GetProperty("children").EnumerateArray())
                //{

                //    var objChild = System.Text.Json.JsonSerializer.Deserialize<TempJsonFuncDto>(childElement.GetRawText(), options);
                //}
                var obj = JsonElementCovertObj<TempJsonFuncDto>(element);
                FunctionsDto dto = new FunctionsDto()
                {
                    Id = i.ToString(),
                    ParentId ="", Content="",Desc=obj.caption,
                    Name = obj.zh, Language="st", Meta="",               
                };

                i++;
                _iFunctionsService.SaveFunctions(dto);
                foreach (JsonElement childElement in element.GetProperty("children").EnumerateArray())
                { 
                    //var objChild = System.Text.Json.JsonSerializer.Deserialize<TempJsonFuncDto>(childElement.GetRawText(), options);
                    var objChild = JsonElementCovertObj<TempJsonFuncDto>(childElement);
                    FunctionsDto dtoChild = new FunctionsDto()
                    {
                        //Id =Guid.NewGuid().ToString(),
                        ParentId =dto.Id,
                        Content = objChild.snippet,
                        Name = objChild.caption,
                        Desc = objChild.zh,
                        Language = "st",
                        Meta = objChild.meta,
                        
                    }; 
                    _iFunctionsService.SaveFunctions(dtoChild); 
                }
                
                Console.WriteLine($"{obj}");
            }

            return await ApiResult.Success(jsonData);
        }

        private T JsonElementCovertObj<T>(JsonElement element)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            T t= System.Text.Json.JsonSerializer.Deserialize<T>(element.GetRawText(), options)!;
            return t;
        }

        private   string GetJsonFile(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }

        private void VisitJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    VisitJsonObject(element);
                    break;
                case JsonValueKind.Array:
                    VisitJsonArray(element);
                    break;
                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    Console.WriteLine(element.GetRawText());
                    break;
                    // 其他情况...
            }
        }

        private void VisitJsonObject(JsonElement element)
        {
            foreach (var property in element.EnumerateObject())
            {
                Console.WriteLine($"Key: {property.Name}");
                VisitJsonElement(property.Value);
            }
        }

        private void VisitJsonArray(JsonElement element)
        {
            foreach (var item in element.EnumerateArray())
            {
                VisitJsonElement(item);
            }
        }
        #endregion

    }
}
