using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;


namespace Rhino.UI
{
  /// <summary>
  /// Used a placeholded which is used by LocalizationProcessor application to create contextId
  /// mapped localized strings
  /// </summary>
  public static class LOC
  {
    ///<summary>
    /// Strings that need to be localized should call this function. The STR function doesn't actually
    /// do anything but return the original string. The LocalizationProcessor application walks
    /// through the source code of a project and looks for LOC.STR. The function is then replaced with a
    /// call to Localization.LocalizeString using a unique context ID.
    ///</summary>
    ///<param name='english'>[in] The english string to localize</param>
    public static string STR(string english)
    {
      return english;
    }

    /// <summary>
    /// Similar to String::Format function
    /// </summary>
    /// <param name="english"></param>
    /// <param name="arg0"></param>
    /// <returns></returns>
    public static string STR(string english, object arg0)
    {
      return string.Format(english, arg0);
    }
  }

  public static class Localization
  {
    /// <summary>
    /// Returns localized version of a given English string. This function should be autogenerated by the
    /// RmaLDotNetLocalizationProcessor application for every function that uses RMASTR
    /// </summary>
    /// <param name="english"></param>
    /// <param name="contextId"></param>
    /// <returns></returns>
    public static string LocalizeString(string english, int contextId)
    {
      Assembly a = Assembly.GetCallingAssembly();
      return LocalizeStringInAssembly(english, contextId, a);
    }

    /// <summary>
    /// Similar to String::Format function
    /// </summary>
    /// <param name="english"></param>
    /// <param name="contextId"></param>
    /// <param name="arg0"></param>
    /// <returns></returns>
    public static string LocalizeString(string english, int contextId, object arg0)
    {
      Assembly a = Assembly.GetCallingAssembly();
      return LocalizeStringInAssembly(english, contextId, arg0, a);
    }


    public static string LocalizeStringInAssembly(string english, int contextId, Assembly assembly)
    {
      return LocalizationUtils.LocalizeString(assembly, CurrentLanguageID, english, contextId);
    }


    public static string LocalizeStringInAssembly(string english, int contextId, object arg0, Assembly assembly)
    {
      string s = LocalizeStringInAssembly(english, contextId, assembly);
      return string.Format(s, arg0);
    }

    ///<summary>
    /// Commands that need to be localized should call this function.
    ///</summary>
    ///<param name='englishCommandName'></param>
    public static string LocalizeCommandName(string englishCommandName)
    {
      Assembly a = Assembly.GetCallingAssembly();
      return LocalizationUtils.LocalizeString(a, CurrentLanguageID, englishCommandName);
    }

/*
    ///<summary>
    /// Command option names that need to be localized should call this function. The RMACON function doesn't actually
    /// do anything but return the original string. The RmaDotNetLocalizationProcessor application walks
    /// through the source code of a project and looks for RHCON. The function is then replaced with a
    /// call to RmaLocalizeCON using a unique context ID.
    ///</summary>
    ///<param name='english_str'>[in] The string to localize</param>
    public static MRhinoCommandOptionName RMACON(string english_str)
    {
      return new MRhinoCommandOptionName(english_str, null);
    }

    public static MRhinoCommandOptionName RMALocalizeCON(string english_str, int context_id)
    {
      string s = Localize.RMALocalizeString(english_str, context_id);
      if (string.IsNullOrEmpty(s))
        s = english_str;
      return new MRhinoCommandOptionName(english_str, s);
    }

    ///<summary>
    /// Command option values that need to be localized should call this function. The RMACOV function doesn't actually
    /// do anything but return the original string. The RmaDotNetLocalizationProcessor application walks
    /// through the source code of a project and looks for RHCON. The function is then replaced with a
    /// call to RmaLocalizeCOV using a unique context ID.
    ///</summary>
    ///<param name='english_str'>[in] The string to localize</param>
    public static MRhinoCommandOptionValue RMACOV(string english_str)
    {
      return new MRhinoCommandOptionValue(english_str, null);
    }

    public static MRhinoCommandOptionValue RMALocalizeCOV(string english_str, int context_id)
    {
      string s = Localize.RMALocalizeString(english_str, context_id);
      if (string.IsNullOrEmpty(s))
        s = english_str;
      return new MRhinoCommandOptionValue(english_str, s);
    }
    */
    ///<summary>
    /// A form or user control should call this in its constructor if it wants to be localized
    /// the typical constructor for a localize form would look like:
    /// MyForm::MyForm()
    /// {
    ///   SuspendLayout();
    ///   InitializeComponent();
    ///   Rhino.UI.Localize.LocalizeForm( this );
    ///   ResumeLayout(true);
    /// }
    ///</summary>
    public static void LocalizeForm(Control form)
    {
      Assembly a = Assembly.GetCallingAssembly();
      LocalizeForm(form, a);
    }

    public static void LocalizeForm(Control form, Assembly assembly)
    {
      LocalizationUtils.LocalizeForm(assembly, CurrentLanguageID, form);
    }

    ///<summary>
    /// A form or user control should call this in its constructor if it wants to localize
    /// context menus that are set on the fly and not assigned to a forms control in design
    /// studio.
    /// MyForm::MyForm()
    /// {
    ///   SuspendLayout();
    ///   InitializeComponent();
    ///   Rhino.UI.Localize.LocalizeToolStripItemCollection( this, this.MyToolStrip.Items );
    /// }
    ///</summary>
    public static void LocalizeToolStripItemCollection(Control parent, ToolStripItemCollection collection)
    {
      LocalizeToolStripItemCollection(parent, collection, Assembly.GetCallingAssembly());
    }

    public static void LocalizeToolStripItemCollection(Control parent, ToolStripItemCollection collection, Assembly assembly)
    {
      LocalizationUtils.LocalizeToolStripItemCollection(assembly, CurrentLanguageID, parent, collection);
    }

    static int m_language_id = -1;
    static int CurrentLanguageID
    {
      get
      {
        // we don't want the language id to change since Rhino in general does not
        // support swapping localizations on the fly. Use a cached language id after the
        // initial language id has been read
        if (m_language_id == -1)
          m_language_id = Rhino.ApplicationSettings.AppearanceSettings.LanguageIdentifier;
        return m_language_id;
      }
    }
  }
}
