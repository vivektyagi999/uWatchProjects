using System;

namespace UwatchPCL.Helpers
{
	public class Constants
	{

        /// The base URL For Production.

        public const string BaseURL = "http://api.uwatch.co.uk/";
        public const bool fortest = false;

        ////Un-Comment above two lines for production


        /// The base URL For Test.

        //public const string BaseURL = "http://apitest.uwatch.co.uk/";
        //public const bool fortest = true;

        //Un-Comment above two lines for test enviornment

        public const int ImageMaxLimit = 20;
	}
    public enum UserRole
    {
        Administrator = 1,
        Demonstrator,
        Agent, 
        Distributor,
        Customer,
        Owner,
        Manufacturer,
        uARM,
        AreaManager,
        InsuranceCompany,
        BeeKeeper,
        PesticideApplier,
        BeeAgent,
        ProfessionalBeeKeeper,
        Bystander
    }
    public enum STATE_CODE
    {
        PRODUCT = 0,
        STOCK = 1,
        REJECTED = 2,
        DEMO = 3,
        SOLD = 4,
        LIVE = 5,
        RETURN = 6,
        DEAD = 8,
        SUSPENDED = 9,
        LOST = 10,
        Repair = 11,
        None = 13
    };

}

