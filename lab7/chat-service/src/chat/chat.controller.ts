import { Controller, Get, Query } from '@nestjs/common';
import { ChatService } from './chat.service';
import { ApiTags, ApiQuery } from '@nestjs/swagger';

@ApiTags('chat')
@Controller('chat')
export class ChatController {
  constructor(private readonly chatService: ChatService) {}

  /**
   * HTTP GET метод для получения истории сообщений.
   * Если параметр room указан, возвращаются сообщения из конкретной комнаты.
   * @param room Название комнаты (опционально).
   * @returns Массив сообщений.
   */
  @Get('history')
  @ApiQuery({ name: 'room', required: false, description: 'Название комнаты чата' })
  async getHistory(@Query('room') room?: string) {
    return this.chatService.getMessages(room);
  }
}
