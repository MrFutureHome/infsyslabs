import { Injectable } from '@nestjs/common';
import { PrismaService } from '../prisma/prisma.service';
import { SendMessageDto } from './dto/send-message.dto';

@Injectable()
export class ChatService {
  constructor(private prisma: PrismaService) {}

  /**
   * Сохраняет сообщение в базе данных.
   * @param dto DTO с информацией о сообщении.
   * @returns Сохраненное сообщение.
   */
  async saveMessage(dto: SendMessageDto) {
    const message = await this.prisma.message.create({
      data: {
        content: dto.content,
        senderId: dto.senderId,
        room: dto.room,
      },
    });
    return message;
  }

  /**
   * Получает историю сообщений. Если задан параметр room, возвращает сообщения из указанной комнаты.
   * @param room Название комнаты (опционально).
   * @returns Массив сообщений.
   */
  async getMessages(room?: string) {
  return this.prisma.message.findMany(
    room ? { where: { room } } : undefined
  );
}
}
