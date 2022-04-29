using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Sync.Directories
{
   public class DirectoriesAll
    {
        public DirectoriesAll(DirectoriesAdd directoriesAdd
            , DirectoriesDelete directoriesDelete
            , DirectoriesReName directoriesReName
            )
        {
            this.directoriesAdd = directoriesAdd;
            this.directoriesDelete = directoriesDelete;
            this.directoriesReName = directoriesReName;
        }

        private DirectoriesAdd directoriesAdd;

        public DirectoriesAdd DirectoriesAdd
        {
            get { return directoriesAdd; }
            set { directoriesAdd = value; }
        }

        private DirectoriesDelete directoriesDelete;

        public DirectoriesDelete DirectoriesDelete
        {
            get { return directoriesDelete; }
            set { directoriesDelete = value; }
        }

        private DirectoriesReName directoriesReName;

        public DirectoriesReName DirectoriesReName
        {
            get { return directoriesReName; }
            set { directoriesReName = value; }
        }

        


    }
}
