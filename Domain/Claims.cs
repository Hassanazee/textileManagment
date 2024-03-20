namespace textileManagment.Domain
{
    public class Claims
    {

        public class Action
        {

            public static string Create = "Create";
            public static string Read = "Read";
            public static string Update = "Update";
            public static string Delete = "Delete";
            public static string Execute = "Execute";
            public static string[] All = new[] { Create, Read, Update, Delete, Execute };

            public static string[] Manage = new[] { Create, Read, Update, Delete };
        }

    }
}
