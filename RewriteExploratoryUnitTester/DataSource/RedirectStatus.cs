namespace RewriteExploratoryUnitTester.DataSource
{
    public enum RedirectStatus
    {
        NotProcessed,
        //Continue, //I think this is basically replaced by Modified... seems to not have been fully implemented?
        Redirected,
        Modified,
        //Restart
    }
}
