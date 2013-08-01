using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DribbbleClient.EntityModels
{
    public class Shot:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public int Likes_count { get; set; }
        public int Comments_count { get; set; }
        public int Rebounds_count { get; set; }
        public string Url { get; set; }

        public string Short_url { get; set; }
        public int Views_count { get; set; }
        public string Rebound_source_id { get; set; }
        public string Image_url { get; set; }

        public string Image_teaser_url { get; set; }
        public string Image_400_url { get; set; }
        public Player Player { get; set; }
        public string Created_at { get; set; }

        private string _comment;
        public string Comment
        {
            get { return this._comment; }
            set 
            {
                this._comment = value;
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Comment"));
            }
        }

        private string _formatDate;
        public string FormatDate
        {
            get { return this._formatDate; }
            set
            {
                this._formatDate = value;
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FormatDate"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
