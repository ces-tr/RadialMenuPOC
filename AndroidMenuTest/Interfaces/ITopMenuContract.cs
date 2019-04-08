

using System;

namespace AndroidMenuTest.Interfaces
 {
    public interface ITopBarComponent
    {
        void Init();

        void HideBackButton();

        void HideTitle();

        void NavigateBack();

        void AddTopBarEvents();

        void RemoveTopBarEvents();

        void SetTitle(string Title);

        event Action OnBackButtonPressedEvent;
    }
}