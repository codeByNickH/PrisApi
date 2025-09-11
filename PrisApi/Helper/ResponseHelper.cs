using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using System.Net;

namespace PrisApi.Helper
{
    public class ResponseHelper
    {
        public static APIResponse CreateApiResponse(ScrapingJob job)
        {
            APIResponse response = new();

            if (!job.Success)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ErrorMessage = new List<string> { job.ErrorMessage };
            }
            else
            {
                response.StatusCode = HttpStatusCode.Created;
            }
            response.IsSuccess = job.Success;
            response.Result = job;
            return response;
        }
        public static APIResponse CreateApiResponse(List<ScrapingJob> jobList)
        {
            APIResponse response = new();
            var test = jobList.Where(e => !string.IsNullOrEmpty(e.ErrorMessage)).ToList();
            
            if (test.Count() > 0)
            {
                var errorList = new List<string>();
                foreach (var error in test)
                {
                    errorList.Add($"{error.Id} - {error.ErrorMessage}");
                }
                response.ErrorMessage = errorList;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                response.StatusCode = HttpStatusCode.Created;
                response.IsSuccess = true;
            }

            response.Result = jobList;
            return response;
        }
    }
}