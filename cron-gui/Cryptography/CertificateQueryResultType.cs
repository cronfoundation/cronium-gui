﻿namespace Cron.Cryptography
{
    internal enum CertificateQueryResultType
    {
        Querying,
        QueryFailed,
        System,
        Missing,
        Invalid,
        Expired,
        Good
    }
}
