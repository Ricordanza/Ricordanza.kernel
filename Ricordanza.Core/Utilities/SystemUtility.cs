using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// システムユーティリティクラスです。
    /// </summary>
    public static class SystemUtility
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// <c>Assembly</c>情報を元にデザインモードか判定します。
        /// </summary>
        public static bool IsDesignMode
        {
            get { return Assembly.GetEntryAssembly() == null; }
        }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// NICからMACアドレスを取得します。
        /// </summary>
        /// <returns>MACアドレス</returns>
        /// <remarks>
        /// ネットワークが接続されていない場合や、稼動しているNICが見つからない場合は <see cref="string.Empty"/> を取得します。
        /// 有線LAN、無線LAN、Bluetoothなど複数のNICが存在する場合は最初に取得したNICからMACアドレスを取得します。
        /// </remarks>
        public static string GetMacAddress()
        {
            // ネットワークが利用できない場合は終了する
            if (!NetworkInterface.GetIsNetworkAvailable())
                return string.Empty;

            var nics = GetNics();

            // 正常に稼働しているイーサネット用のNICが存在しない場合
            if (nics == null)
                return string.Empty;

            // 最初のNICのMACアドレスを返却
            return nics.FirstOrDefault<NetworkInterface>().GetPhysicalAddress().ToString();
        }

        /// <summary>
        /// 有効なイーサネット用のNICの一覧を取得する。
        /// </summary>
        /// <returns>有効なイーサネット用のNIC</returns>
        public static IEnumerable<NetworkInterface> GetNics()
        {
            // 正常に稼働しているイーサネット用のNICのみを取得
            return NetworkInterface.GetAllNetworkInterfaces().Where<NetworkInterface>(
                    (n) => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet && n.OperationalStatus == OperationalStatus.Up
                );
        }

        /// <summary>
        /// テンポラリフォルダに作成したExcelファイルを削除します。
        /// </summary>
        public static void RemoveExcelTemp()
        {
            string[] files = Directory.GetFiles(Path.GetTempPath(), "{0}*.xls".DirectFormat(System.Windows.Forms.Application.ProductName));

            files.ToList().ForEach(
                file =>
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                });
        }

        /// <summary>
        /// <c>Windows Vista</c>以降(<c>UAC</c>搭載<c>OS</c>)で起動されているか判定します。
        /// </summary>
        /// <returns><c>Windows Vista</c>以降(<c>UAC</c>搭載<c>OS</c>)で起動されている場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        /// <remarks><c>Windows 7</c>,<c>Windows Server 2008 R2</c>までサポート</remarks>
        public static bool OsWithUAC()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            if (osInfo.Platform == PlatformID.Win32NT)
            {
                if (osInfo.Version.Major == 6)
                {
                    if (osInfo.Version.Minor == 0)
                        // Windows Vista, Windows Server 2008
                        return true;
                    else if (osInfo.Version.Minor == 1)
                        // Windows 7, Windows Server 2008 R2
                        return true;
                }
                else if (osInfo.Version.Major > 6)
                    // new Windows
                    return true;
            }

            return false;
        }

        /// <summary>
        /// アプリケーションが管理者権限で起動されているか判定します。
        /// </summary>
        /// <returns>アプリケーションが管理者権限で起動されている場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        /// <remarks>処理が重たい。。。</remarks>
        public static bool RunningOnAdministrators()
        {
            bool isAllowed = false;

            try
            {
                UserPrincipal.Current.GetGroups().ToList().ForEach(gp =>
                {
                    using (gp)
                    {
                        if (gp.Name.ToLower() == "Administrators".ToLower())
                        {
                            isAllowed = true;
                            return;
                        }
                    }
                });
            }
            catch
            {
                isAllowed = false;
            }

            return isAllowed;
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
