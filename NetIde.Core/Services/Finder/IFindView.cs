using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.Services.Finder
{
    internal interface IFindView : IServiceProvider
    {
        string GetFindWhatText();
        string GetLookAtFileTypesText();
        string GetLookInText();
        string GetReplaceWithText();
        string[] GetFindWhatHistory();
        string[] GetLookAtFileTypesHistory();
        string[] GetLookInHistory();
        string[] GetReplaceWithHistory();
        void BeginUpdate();
        void EndUpdate();
        void LoadFindWhatHistory(string[] history);
        void LoadLookAtFileTypesHistory(string[] history);
        void LoadLookInHistory(string[] history);
        void LoadReplaceWithHistory(string[] history);
        void NoMoreOccurrences();
        void SetIncludeSubFolders(bool value);
        void SetKeepOpen(bool value);
        void SetMatchCase(bool value);
        void SetMatchWholeWord(bool value);
        void SetMode(FindMode findMode);
        void SetTarget(FindTarget findTarget);
        void SetUseRegularExpressions(bool value);
    }
}
