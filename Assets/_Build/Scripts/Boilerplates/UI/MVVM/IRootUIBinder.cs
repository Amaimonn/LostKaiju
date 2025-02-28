using System.Collections.Generic;
using LostKaiju.Boilerplates.Locator;

namespace LostKaiju.Boilerplates.UI.MVVM
{
    public interface IRootUIBinder : IService
    {
        public void SetView(View view);

        public void SetViews(IEnumerable<View> views);

        public void SetViews(params View[] views);

        public void AddView(View view);

        public void AddViews(params View[] views);

        public void AddViews(IEnumerable<View> views);
        
        public void ClearView(View view);

        public void ClearViews();
    }
}