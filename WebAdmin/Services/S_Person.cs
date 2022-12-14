﻿using Student_WebAdmin.Lib;
using Student_WebAdmin.Models;
using System;

namespace Student_WebAdmin.Services
{
    public interface IS_Person
    {
        Task<ResponseData<List<M_Person>>> getListPerson(string accessToken);
        Task<ResponseData<List<M_Person>>> getListPersonByConditionSequenceStatus(string accessToken, string sequenceStatus, string name, DateTime? fdate, DateTime? tdate);
        Task<ResponseData<List<M_Person>>> getListPersonBySequenceStatus(string accessToken, string sequenceStatus);
        Task<ResponseData<M_Person>> getPersonById(string accessToken, int id);
        Task<ResponseData<M_Person>> Create(string accessToken, EM_Person model, string createdBy);
        Task<ResponseData<M_Person>> Update(string accessToken, EM_Person model, string updatedBy);
        Task<ResponseData<M_Person>> Delete(string accessToken, int id);
        Task<ResponseData<M_Person>> UpdateStatus(string accessToken, int id, int status, DateTime? timer, string updatedBy);
    }
    public class S_Person : IS_Person
    {
        private readonly ICallBaseApi _callApi;
        private readonly IS_Address _s_Address;
        public S_Person(ICallBaseApi callApi, IS_Address s_Address)
        {
            _callApi = callApi;
            _s_Address = s_Address;
        }

        public async Task<ResponseData<List<M_Person>>> getListPerson(string accessToken)
        {
            return await _callApi.GetResponseDataAsync<List<M_Person>>("/Person/getListPerson", default(Dictionary<string, dynamic>), accessToken);
        }
        //public async Task<ResponseData<List<M_Person>>> getListPerson(string accessToken)
        //{
        //    return await _callApi.GetResponseDataAsync<List<M_Person>>("Person/getListPerson", default(Dictionary<string, dynamic>), accessToken);
        //}
        public async Task<ResponseData<List<M_Person>>> getListPersonByConditionSequenceStatus(string accessToken, string sequenceStatus, string name, DateTime? fdate, DateTime? tdate)
        {
            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"sequenceStatus", sequenceStatus},
                {"name", name},
                {"fdate", fdate?.ToString("O")},
                {"tdate", tdate?.ToString("O")},
            };
            return await _callApi.GetResponseDataAsync<List<M_Person>>("Person/getListPersonByConditionSequenceStatus", dictPars, accessToken);
        }
        public async Task<ResponseData<List<M_Person>>> getListPersonBySequenceStatus(string accessToken, string sequenceStatus)
        {
            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"sequenceStatus", sequenceStatus},
            };
            return await _callApi.GetResponseDataAsync<List<M_Person>>("/Person/getListPersonBySequenceStatus", dictPars, accessToken);
        }
        public async Task<ResponseData<M_Person>> getPersonById(string accessToken, int id)
        {
            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"id", id},
            };
            return await _callApi.GetResponseDataAsync<M_Person>("/Person/getPersonById", dictPars, accessToken);
        }
        public async Task<ResponseData<M_Person>> Create(string accessToken, EM_Person model, string createdBy)
        {

            var cAddress = await _s_Address.Create(accessToken, model.addressObj, createdBy);
            model.addressId = cAddress.data.Id;

            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"firstName", model.firstName},
                {"lastName", model.lastName},
                {"personTypeId",model.personTypeId },
                { "birthDay", model.birthDay},
                {"gender", model.gender},
                {"nationalityId",model.nationalityId }, 
                {"religionId",model.religionId },
                {"folkId",model.folkId },
                {"addressId",model.addressId },
                {"phoneNumber",model.phoneNumber },
                {"email", model.email},
                //{"createdBy", createdBy},
            };
            return await _callApi.PostResponseDataAsync<M_Person>("/Person/Create", dictPars, accessToken);
        }
        public async Task<ResponseData<M_Person>> Update(string accessToken, EM_Person model, string updatedBy)
        {
            var res = new ResponseData<M_Person>();
            if (model.addressId != null && model.addressId != 0)
            {
                var resAddress = await _s_Address.Update(accessToken, model.addressObj, updatedBy);
                if (resAddress.result != 1 || resAddress.data == null)
                {
                    res.result = resAddress.result;
                    res.error = resAddress.error;
                    return res;
                }
            }
            else
            {
                var resAddress = await _s_Address.Create(accessToken, model.addressObj, updatedBy);
                if (resAddress.result == 1 && resAddress.data != null)
                    model.addressId = resAddress.data.Id;
                else
                {
                    res.result = resAddress.result; 
                    res.error = resAddress.error;
                    return res;
                }
            }

            
            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"id", model.id},
                {"firstName", model.firstName},
                {"lastName", model.lastName},
                {"gender", model.gender},
                {"phoneNumber", model.phoneNumber},
                {"status", model.status},
                //{"updatedBy", updatedBy},
                {"timer", model.timer?.ToString("O")},
                //{"timer",DateTime.Now.ToString("mm-dd-yyyy") },
                {"addressId",model.addressId },
                {"personId",model.personTypeId }
            };
            return await _callApi.PutResponseDataAsync<M_Person>("/Person/Update", dictPars, accessToken);
        }
        public async Task<ResponseData<M_Person>> Delete(string accessToken, int id)
        {
            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"id", id},
            };
            return await _callApi.DeleteResponseDataAsync<M_Person>("/Person/Delete", dictPars, accessToken);
        }
        public async Task<ResponseData<M_Person>> UpdateStatus(string accessToken, int id, int status, DateTime? timer, string updatedBy)
        {
            Dictionary<string, dynamic> dictPars = new Dictionary<string, dynamic>
            {
                {"id", id},
                {"status", status},
                {"updatedBy", updatedBy},
                {"timer", timer?.ToString("O")},
            };
            return await _callApi.PutResponseDataAsync<M_Person>("/Person/UpdateStatus", dictPars, accessToken);
        }
    }
}
