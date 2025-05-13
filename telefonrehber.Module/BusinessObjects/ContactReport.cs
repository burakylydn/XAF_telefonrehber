using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

[DefaultClassOptions]
public class ContactReport : BaseObject
{
    public ContactReport(Session session) : base(session) { }

    private DateTime startDate;
    private DateTime endDate;

    public DateTime StartDate
    {
        get => startDate;
        set => SetPropertyValue(nameof(StartDate), ref startDate, value);
    }

    public DateTime EndDate
    {
        get => endDate;
        set => SetPropertyValue(nameof(EndDate), ref endDate, value);
    }
}
