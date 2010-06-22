var fields = 'id,name,link,facebookUid';
var thweetFields = '[id,text,{author:[facebookUid,name]},date]';
var profile;
var getUserProfileCalled = false;
var thweetTemplate;
var authorId;

$(function() {
	authorId = document.location.hash.substr(1);
	setupFacebook();
	setupNewThweeter();
	setupList();
	$('input[hint]').hint();
});

function getUserProfile(session)
{
	if( getUserProfileCalled ) return;
	getUserProfileCalled = true;
	$.rest.get('/api/model/userProfile/' + authorId,'fields=' + fields, function(resp) {
		profile = resp;
		$('h1').append(resp.name);
		$('img#userLogo').attr('src','http://graph.facebook.com/' + resp.facebookUid + '/picture');
		setupList();
	});
}

function setupNewThweeter()
{
	thweetTemplate = new JTMLTemplate($('script#thweetTemplate'));
	$('#postThweet').click(function() {
		var text = $('#newThweet').val();
		if( text.length > 3 )
		{
			alert('Oops!  You tried to thweet more than three letters!');
			return;
		}
		var data = {
			text : text,
			author : profile.id
		}
		$.rest.post( '/api/model/thweet/?fields=' + thweetFields, data, function(resp) {
			$.extend(data,resp);
			var newEntry = $(thweetTemplate.render({thweet:data}));
			newEntry.find('abbr.timeago').timeago();
			$('#main').prepend(newEntry);
		});
	});
	
	$('#newThweet').val('').keyup(function() {
		var length = $(this).val().length;
		$('label#count').html(3-length);
		if( length > 3 )
			$('label#count').addClass('error');
		else
			$('label#count').removeClass('error');
	});
}

function setupList()
{
	$.rest.get('/api/model/thweet/author/' + authorId, 'fields=' + thweetFields, function(list) {
		$.each(list.thweets, function(i,v) {
			$('#main').prepend(thweetTemplate.render({thweet:v}))
		});
		$('abbr.timeago').timeago();
	});	
}

function setupFacebook()
{
 	window.fbAsyncInit = function() {
         FB.init({appId: '133006173388381', status: true, cookie: true,
                 xfbml: true});
     	 FB.getLoginStatus(function(response) {
			  if (response.session) {
				getUserProfile(response.session);
			  } else {
				console.log('not logged in.');
			  }
		 });
		 FB.Event.subscribe('auth.sessionChange', function(response) {
			if (response.session) {
			  getUserProfile(response.session);
			} else {
				console.log(response);
			}
		  });
      };
      (function() {
        var e = document.createElement('script');
        e.type = 'text/javascript';
        e.src = document.location.protocol +
          '//connect.facebook.net/en_US/all.js';
        e.async = true;
        document.getElementById('fb-root').appendChild(e);
      }());
}
