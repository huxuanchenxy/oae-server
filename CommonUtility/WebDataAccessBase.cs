
using System.Net;

namespace CommonUtility
{
    public class WebDataAccess
    {
        protected string domain = "http://10.89.34.158:61499";
        //protected string domain = "http://localhost:5285/test";
        public Task<string> GetDatas(string url)
        {
            using (HttpClient client=new HttpClient ())
            {
                var resp=client.GetAsync($"{domain}{url}").GetAwaiter ().GetResult ();  
                return resp.Content.ReadAsStringAsync ();
            }
        }

        private MultipartFormDataContent GetFormData(Dictionary<string,HttpContent> contents)
        {
            var postContent=new MultipartFormDataContent ();
            string boundary = $"---{DateTime.Now.Ticks.ToString("x")}---";
            postContent.Headers.Add("ContentType", $"muiltipart/form-data,boundary={boundary}");
            foreach (var item in contents) {
                postContent.Add(item.Value, item.Key);
            }
            return postContent;
        }

        public Task<string> PostDatas(string url, Dictionary<string, HttpContent> contents)
        {
            using (HttpClient client = new HttpClient())
            {
                var resp = client.PostAsync($"{domain}{url}", GetFormData(contents)).GetAwaiter().GetResult();
                return resp.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PostDatas(string url, HttpContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                //var c = new System.Net.Http.MultipartFormDataContent();
                var resp = await client.PostAsync($"{domain}{url}", content); 
                return await resp.Content.ReadAsStringAsync();

                
            }
        }


    }
}