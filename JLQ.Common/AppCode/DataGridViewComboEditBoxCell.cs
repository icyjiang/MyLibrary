using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace JLQ.Common
{
    //扩展的DGV列ComboBox，可编辑也可以拉选
    public class DataGridViewComboEditBoxCell : DataGridViewComboBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            ComboBox comboBox = base.DataGridView.EditingControl as ComboBox;
            if (comboBox != null)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.Validating += new CancelEventHandler(comboBox_Validating);
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            if (value != null && DataSource == null)
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    if (Items.IndexOf(value) == -1)
                    {
                        Items.Add(value);
                        DataGridViewComboBoxColumn col = OwningColumn as DataGridViewComboBoxColumn;
                        col.Items.Add(value);
                    }
                }
            }
            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter,
                formattedValueTypeConverter, context);
        }

        void comboBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataGridViewComboBoxEditingControl cbo = sender as DataGridViewComboBoxEditingControl;
            if (string.IsNullOrWhiteSpace(cbo.Text)) return;

            DataGridView grid = cbo.EditingControlDataGridView;
            object value = cbo.Text;

            if (cbo.Items.IndexOf(value) == -1)
            {
                DataGridViewComboBoxColumn cboCol = grid.Columns[grid.CurrentCell.ColumnIndex] as DataGridViewComboBoxColumn;
                if (DataSource == null)
                {
                    cbo.Items.Add(value);
                    cboCol.Items.Add(value);
                    grid.CurrentCell.Value = value;
                }
            }
        }
    }

    public class DataGridViewComboEditBoxColumn : DataGridViewComboBoxColumn
    {
        public DataGridViewComboEditBoxColumn()
        {
            DataGridViewComboEditBoxCell obj = new DataGridViewComboEditBoxCell();
            this.CellTemplate = obj;
        }
    }

}
