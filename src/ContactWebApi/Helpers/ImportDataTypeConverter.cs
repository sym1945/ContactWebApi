﻿using ContactWebApi.Constants;
using ContactWebApi.Domain.Enums;

namespace ContactWebApi.Helpers
{
    public static class ImportDataTypeConverter
    {
        public static EImportDataType CovertFrom(string? contentType)
        {
            switch (contentType)
            {
                case ContentTypes.ApplicationJson: return EImportDataType.Json;
                case ContentTypes.TextCsv: return EImportDataType.Csv;
                default: return EImportDataType.Unknown;
            }
        }

    }
}
