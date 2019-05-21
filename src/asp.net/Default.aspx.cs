using System;
using System.Web.UI;

namespace asp.net
{

    public partial class _Default : Page
    {

        private EmployeeRepository _repository;

        private EmployeeRepository Repository => _repository ?? (_repository = new EmployeeRepository());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            string employeeId = RouteData.Values["id"] as string;
            if (employeeId == "*" || string.IsNullOrEmpty(employeeId))
            {
                GridViewEmployees.DataSource = Repository.GetEmployees();
                GridViewEmployees.DataBind();
                DetailsViewEmployee.Visible = false;
            } else
            {
                var employees = Repository.GetEmployees(employeeId);
                DetailsViewEmployee.DataSource = employees;
                DetailsViewEmployee.DataBind();
                GridViewEmployees.Visible = false;
            }
        }

    }

}