﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Trivia_Client
{
    class RequestHandler
    {
        private static int getRoomId(String roomName)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.GET_ROOMS;
            Responses.ResponseInfo responseInfo = Communicator.sendRequest(buffer);
            GetRoomsResponse response =  Deserializer.DeserialiseResponse<GetRoomsResponse>(responseInfo.Buffer);
            List<RoomData> rooms = Deserializer.DeserialiseResponse<List<RoomData>>(response.rooms);
            for(int i = 0; i <rooms.Count; i++)
            {
                if (rooms[i].Name.equals(roomName)) // if this is the room we're looking for
                    return rooms[i].Id;
            }
            return -1; // if no room was found
            
        }
        private static void HandleRequest(byte[] buffer, Form form)
        {
            Responses.ResponseInfo response = Communicator.sendRequest(buffer);
            ResponseHandler.HandelResponse(response, form);
        }
        public static void Logout(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.LOGOUT;
            HandleRequest(buffer, form);
        }
        public static void Signup(String username, String password, String email, String phone, String address, String birthdate, Form form)
        {
            Requests.SignupRequest request = new Requests.SignupRequest { Username = username, Password = password, PhoneNumber = phone,
                                                                          Email = email, Address = address , BirthDate = birthdate};
            HandleRequest(Serializer.SerializeRequest(request), form);
        }
        public static void Login(String username, String password , Form form)
        {
            Requests.LoginRequest request = new Requests.LoginRequest { Username = username, Password = password };
            HandleRequest(Serializer.SerializeRequest(request), form);
        }
        public static void GetRooms(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.GET_ROOMS;
            HandleRequest(buffer, form);
        }
        public static void GetPlayersInRoom(String roomName, Form form)
        {
            Requests.GetPlayersInRoomRequest request = new Requests.GetPlayersInRoomRequest { RoomId = getRoomId(roomName) };
            HandleRequest(Serializer.SerializeRequest(request), form);
        }
        public static void GetStatistics(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.GET_STATISTICS;
            HandleRequest(buffer, form);
        }
        public static void JoinRoom(String roomName, Form form)
        {
            Requests.JoinRoomRequest request = new Requests.JoinRoomRequest { RoomId = getRoomId(roomName) };
            HandleRequest(Serializer.SerializeRequest(request), form);
        }
        public static void CreateRoom(String roomName, int maxUsers, int questionCount, int answerTimeout, Form form)
        {
            Requests.CreateRoomRequest request = new Requests.CreateRoomRequest { RoomName = roomName, MaxUsers = maxUsers, QuestionCount = questionCount, AnswerTimeout = answerTimeout};
            HandleRequest(Serializer.SerializeRequest(request), form);
        }
        public static void CloseRoom(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.CLOSE_ROOM;
            HandleRequest(buffer, form);
        }
        public static void StartGame(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.START_GAME;
            HandleRequest(buffer, form);
        }
        public static void GetRoomState(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.GET_ROOM_STATE;
            HandleRequest(buffer, form);
        }
        public static void LeaveRoom(Form form)
        {
            byte[] buffer = new byte[Serializer.CODE_SIZE + Serializer.LEN_SIZE];
            buffer[0] = (byte)Serializer.codeId.LEAVE_ROOM;
            HandleRequest(buffer, form);
        }


    }
}
