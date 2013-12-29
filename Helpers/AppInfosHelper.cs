using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Wp8Shared.Helpers
{
    public class AppInfosHelper
    {
        public static Guid GetId()
        {
            Guid applicationId = Guid.Empty;

            var productId = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("ProductID");

            if (productId != null && !string.IsNullOrEmpty(productId.Value))
                Guid.TryParse(productId.Value, out applicationId);

            return applicationId;
        }

    }
}
