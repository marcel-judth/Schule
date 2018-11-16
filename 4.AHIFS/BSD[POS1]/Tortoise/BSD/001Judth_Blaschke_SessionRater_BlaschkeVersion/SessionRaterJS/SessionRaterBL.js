﻿"use strict";

var Session             = require("./Session.js");
var Rating              = require("./Rating.js");
var SessionException    = require("./SessionException.js");
var SessionState        = require("./SessionState.js");


module.exports = (function () {

    //fields
    var nextID = 1;
    var sessions = [];
    
    //methods
    function _createTestData() {
        _createSession("A", "sdafasd");
        _createSession("B", "sdafasd");
        _createSession("C", "sdafasd");
        _createSession("D", "sdafasd");
        _createSession("E", "sdafasd");
        _createSession("F", "sdafasd");

    }

    function _createSession(title, speaker) {
      return _validate({'title' : title     , 'speaker' : speaker },
                       {'title' : "string"  , 'speaker' : "string"},
                function(){
                  if(_titleAlrreadyInList(title))
                    throw new SessionException("This title appears to be already taken");
                  var session = new Session(nextID++, title, speaker);
                  sessions.push(session);
                  return session;
                }
              );
    }

    function _getSessionByID(param) {
        if (typeof param != "number")
            throw new SessionException("this param is not valid");

        var res = null;
        res = sessions.find(function (val, index, arr) {
            return (val.ID == param);
        });
        if (!res)
            throw new SessionException("no matching session was found");
        return res;
    }

    function _getAll() {
      return sessions;
    }

    function _deleteSession(sessionID) {
      _validate({'sessionID' : sessionID},
                {'sessionID' : "number"},
                function(){
                  var session = _getSessionByID(sessionID);
                  var index   = sessions.indexOf(session);
                  if(index > -1 )
                    sessions.splice(index,1)
                  else
                    throw new SessionException("This element isnt available anymore!","Removing Session Excpetion");
                }
              );
    }

    function _rateSession(sessionID, RatingValue, Evaluator) {
      _validate({'sessionID' : sessionID , 'Evaluator' : Evaluator},
                {'sessionID' : "number"  , 'Evaluator' : "string"},
                function(){
                  var session = _getSessionByID(sessionID);
                  if(session.currentSessionState == SessionState.CLOSED)
                    throw new SessionException("this Session has already been closed");
                  session.rate(RatingValue,Evaluator,session);
                }
              );
    }

    function _closeSession(sessionID) {
      _validate({'sessionID' : sessionID},
                {'sessionID' : "number"},
                function(){
                  var session = _getSessionByID(sessionID);
                  session.currentSessionState = SessionState.CLOSED;
                }
              );
    }

    //return object
    return {

        createSession   : _createSession,
        deleteSession   : _deleteSession,
        closeSession    : _closeSession,
        getSessionByID  : _getSessionByID,
        getSessions     : _getAll,
        rateSession     : _rateSession,
        createTestData  : _createTestData

    };


    //helping methods...

    function _validate(param,paramtype,callback){
      try{
        for(var key in param)
          if(typeof param[key] != paramtype[key])
            throw new SessionException("given param is not valid","invalid type Session Exception");

      }catch(ex){
        throw new SessionException("given param is not valid : " + ex, "invalid type Session Exception");
      }

      try{
        if(typeof callback != "function")
          throw new SessionException("callback function is not type of function","invalid type Session Exception");
        return callback();
      }catch(ex){
        throw new SessionException("An error occured : " + ex.toString());
      }

    }

    function _titleAlrreadyInList(title){
      return sessions.some(function(value,index,arr){
        return (value.title == title);
      });
    }


})();