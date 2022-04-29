using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Sync.File
{
  public  class FileAll
    {
        public FileAll(FileAdd fileAdd
            , FileDelete fileDelete
            , FIleUpdate fIleUpdate
            )
        {
            this.fileAdd = fileAdd;
            this.fileDelete = fileDelete;
            this.fIleUpdate = fIleUpdate;
        }

        private FileAdd fileAdd;

        public FileAdd FileAdd
        {
            get { return fileAdd; }
            set { fileAdd = value; }
        }

        private FileDelete fileDelete;

        public FileDelete FileDelete
        {
            get { return fileDelete; }
            set { fileDelete = value; }
        }


        private FIleUpdate fIleUpdate;

        public FIleUpdate FIleUpdate
        {
            get { return fIleUpdate; }
            set { fIleUpdate = value; }
        }

    }
}
