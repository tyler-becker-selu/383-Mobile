using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CustomerConsumer
{
	public class OnLoginEventArgs:EventArgs
	{
		private String globUserName;
		private String globPassword;

		public String UserName
		{
			get{ return globUserName; }
			set{ globUserName = value; }
		}
		public String Password
		{
			get{ return globPassword; }
			set{ globPassword = value; }
		}
		public OnLoginEventArgs(String uName, String pass) : base()
		{
			UserName = uName;
			Password = pass;
		}
	}
	class DialogLogin : DialogFragment
	{
		private EditText globUserNameText;
		private EditText globPasswordText;
		private Button globLoginSubmitBTN;
		public event EventHandler<OnLoginEventArgs> globOnLoginComplete;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Dialog.SetTitle ("Login");

			base.OnCreateView(inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.DialogLoginLayout,container, false);
			globUserNameText = view.FindViewById<EditText> (Resource.Id.UserNameText);
			globPasswordText = view.FindViewById<EditText> (Resource.Id.PasswordText);
			globLoginSubmitBTN = view.FindViewById<Button> (Resource.Id.LoginSubmitBTN);

			globLoginSubmitBTN.Click+= GlobLoginSubmitBTN_Click;
			return view;
		}

		void GlobLoginSubmitBTN_Click (object sender, EventArgs e)
		{
				
			//user clicked login submit button
			if (globUserNameText.Length () == 0 || globPasswordText.Length () == 0) {
				if (globPasswordText.Length () == 0) {
					globPasswordText.SetError("Password is Required", null);
				}
				if (globUserNameText.Length () == 0) {
					globUserNameText.SetError("Email Address is Required", null);
				}
			} else {
				globOnLoginComplete.Invoke (this, new OnLoginEventArgs (globUserNameText.Text, globPasswordText.Text));
				this.Dismiss ();
			}
		}
	}
}

