using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using MyShelf_Business;
using MyShelf_Web.Model;

namespace MyShelf_Web.Pages.Account
{
    public class SigninModel : PageModel
    {
        [BindProperty]
        public Login LoginUser { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Validate User Input
                // Check if the user exists in the database
                // If the user exists, redirect to the profile page
                // If the user does not exist, display an error message
                using (SqlConnection conn = new SqlConnection(AppHelper.GetDBConnectionString()))
                {
                    string cmdText = "SELECT UserID, UserPassword FROM [User] WHERE UserEmail = @email";
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@email", LoginUser.Email);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string passwordHash = reader.GetString(1);
                        if (AppHelper.VerifyPassword(LoginUser.Password, passwordHash))
                        {
                            return RedirectToPage("/Account/Profile");
                        }
                        else
                        {
                            ModelState.AddModelError("LoginError", "Invalid credentials.");
                            return Page();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LoginError", "Invalid credentials.");
                        return Page();
                    }
                }
            }
            else
            {
                return Page();
            }
        }
    }
}
