import {
  WebSocketGateway,
  SubscribeMessage,
  MessageBody,
  WebSocketServer,
} from '@nestjs/websockets';
import { Server } from 'socket.io';
import { ChatService } from './chat.service';
import { SendMessageDto } from './dto/send-message.dto';

@WebSocketGateway({ cors: true }) // Разрешаем кросс-доменные запросы
export class ChatGateway {
  @WebSocketServer()
  server: Server;

  constructor(private chatService: ChatService) {}

  /**
   * Обработчик входящих сообщений через WebSocket.
   * При получении события 'sendMessage' сохраняет сообщение в базе и транслирует его всем подключенным клиентам.
   * @param data Данные сообщения, переданные клиентом.
   * @returns Сохраненное сообщение.
   */
  @SubscribeMessage('sendMessage')
  async handleSendMessage(@MessageBody() data: SendMessageDto) {
    // Сохраняем сообщение в базу данных
    const savedMessage = await this.chatService.saveMessage(data);

    // Трансляция сообщения всем подключенным клиентам
    this.server.emit('message', savedMessage);
    return savedMessage;
  }
}
