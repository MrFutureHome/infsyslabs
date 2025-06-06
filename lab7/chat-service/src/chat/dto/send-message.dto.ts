import { ApiProperty } from '@nestjs/swagger';
import { IsNotEmpty, IsString, IsNumber } from 'class-validator';

export class SendMessageDto {
  @ApiProperty({ example: 'Привет, как дела?' })
  @IsNotEmpty({ message: 'Содержание сообщения не должно быть пустым' })
  @IsString({ message: 'Содержание сообщения должно быть строкой' })
  content: string;

  @ApiProperty({ example: 123, description: 'ID пользователя-отправителя' })
  @IsNotEmpty({ message: 'ID отправителя обязателен' })
  @IsNumber({}, { message: 'ID отправителя должен быть числом' })
  senderId: number;

  @ApiProperty({ example: 'general', description: 'Название комнаты или чата', required: false })
  room?: string;
}
