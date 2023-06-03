﻿namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class EmployeeImportResult
    {
        public int GroupId { get; init; }
        public int Count { get; init; }

        public EmployeeImportResult(int groupId, int count)
        {
            GroupId = groupId;
            Count = count;
        }
    }
}
