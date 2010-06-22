using System;
using EmergeTk.Model;
using System.Net;
using System.IO;
using EmergeTk;
using EmergeTk.WebServices;
using System.Collections.Generic;

namespace Thweeter
{
	[RestService(ModelName="userProfile",ServiceManager=typeof(DefaultServiceManager))]
	public class UserProfile : AbstractRecord
	{
		long facebookUid;
		string name;
		string link;
		string accessToken;

		public long FacebookUid {
			get {
				return this.facebookUid;
			}
			set {
				facebookUid = value;
			}
		}

		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}
		
		public string Link {
			get {
				return this.link;
			}
			set {
				link = value;
			}
		}

		public string AccessToken {
			get {
				return this.accessToken;
			}
			set {
				accessToken = value;
			}
		}	
		
		public override void Save (bool SaveChildren, bool IncrementVersion, System.Data.Common.DbConnection conn)
		{
			if( Name == null && facebookUid > 0 )
			{
				WebClient wc = new WebClient();
				string url = "http://graph.facebook.com/" + facebookUid;
				log.Debug("sending url to facebook", url);
				StreamReader sr = new StreamReader
					(wc.OpenRead(url));
				string json = sr.ReadToEnd();
				var obj = JSON.Default.DecodeObject(json);
				this.name = (string)obj["name"];
				this.link = (string)obj["link"];
			}
			base.Save (SaveChildren, IncrementVersion, conn);
		}
		
		public UserProfile ()
		{
		}
	}
}

