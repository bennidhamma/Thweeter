using System;
using System.Collections.Generic;
using EmergeTk.Model;
using EmergeTk.WebServices;

namespace Thweeter
{
	[RestService(ModelName="thweet",ServiceManager=typeof(DefaultServiceManager))]
	public class Thweet : AbstractRecord
	{
		private UserProfile author;		
		public UserProfile Author
		{
			get 
			{
				CheckProperty("Author",author);
				return author;
			}
			set
			{
				author = value;
			}
		}
		
		public string Text { get; set; }
		
		public DateTime Date { get; set; }
		
		public Thweet()
		{
			Date = DateTime.Now;	
		}
		
		public override List<ValidationError> Validate (string path, List<ValidationError> errors)
		{
			if( author == null )
			{
				if( errors == null )
					errors = new List<EmergeTk.Model.ValidationError>();
				errors.Add(new ValidationError() {
					Problem = "Author is null."
				});
			}
			if( Text == null )
			{
				if( errors == null )
					errors = new List<EmergeTk.Model.ValidationError>();
				errors.Add(new ValidationError() {
					Problem = "Text is null."
				});
			}
			else if( Text > 3 )
			{
				if( errors == null )
					errors = new List<EmergeTk.Model.ValidationError>();
				errors.Add(new ValidationError() {
					Problem = "Text cannot be greater than three characters."
				});
			}
			return base.Validate (path, errors);
		}
	}
}

