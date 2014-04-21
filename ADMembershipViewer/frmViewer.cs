using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ADMembershipViewer
{
    public partial class frmViewer : Form
    {
        public frmViewer()
        {
            InitializeComponent();
        }

        private void frmViewer_Load(object sender, EventArgs e)
        {
            Icon = Icon.FromHandle(Properties.Resources.groupIcon.GetHicon());

            gridView.AutoGenerateColumns = false;
            gridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", Name = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            gridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DistinguishedName", Name = "Distinguished Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            gridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Sid", Name = "Security Identifier (SID)", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            gridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Guid", Name = "GUID", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            gridView.RowHeadersVisible = false;
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtUsername.Enabled = false;
            btnSearch.Enabled = false;
            Application.DoEvents();
            gridView.DataSource = getGroups(txtUsername.Text);
            txtUsername.Enabled = true;
            btnSearch.Enabled = true;
        }

        private List<GroupPrincipal> getGroups(string userName)
        {
            var result = new List<GroupPrincipal>();
            if (userName == string.Empty)
                return result;

            var domainContext = new PrincipalContext(ContextType.Domain);
            var user = UserPrincipal.FindByIdentity(domainContext, userName);

            if (user == null)
                return result;

            var groups = user.GetAuthorizationGroups();
            result.AddRange(groups.OfType<GroupPrincipal>());
            return result.OrderBy(g => g.Name).ToList();
        }
    }
}
