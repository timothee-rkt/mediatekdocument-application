using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    public class FrmAlerteAbonnements : Form
    {
        public FrmAlerteAbonnements(List<CommandeDocument> abonnements)
        {
            Text = "Abonnements à renouveler";
            Width = 600;
            Height = 400;
            StartPosition = FormStartPosition.CenterScreen;

            var dgv = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = false };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Revue", DataPropertyName = "IdDocument", Width = 180 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date fin abonnement", DataPropertyName = "DateFinAbonnement", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Montant", DataPropertyName = "Montant", Width = 100 });
            dgv.DataSource = abonnements;
            Controls.Add(dgv);
        }
    }
}
