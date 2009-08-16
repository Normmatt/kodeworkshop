.arm
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

@ CHEAT ENGINE	

@ R12:	CodeTableStart
@ R11:	Value
@ R10:	Code
@ R0-R9 are generic registers for the code processor to use
	
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

CB_start:
	stmdb 	r13!, {r0-r12, lr}

	ldr		r12, codeTableStart

@	r0-r9 are generic registers for the code processor to use
@	0xCF000000 0x00000000 indicates the end of the code list
	
main_loop:
	ldmia	r12!, {r10, r11}		@ load a code
	cmp 	r10, #0xCF000000
	beq		CB_return
	
	mov		r0,	r10, lsr #24
	and 	r0, #0xF8

@ check code group
	cmp		r0, 	#0xD0
	beq		if_code
	cmp		r0,		#0x30
	bls		raw_write
	cmp		r0,		#0x40
	blt		increment_write
	beq		slider_write
	b 		CB_return
	
@@@@@@@@@@@@@@@@
@ type 0x00-0x20 

raw_write:
	cmp		r0, #0x10
	bic		r10, r10, #0xF8000000
	strltb	r11, [r10, r9]		@ type 0x00
	streqh	r11, [r10, r9]		@ type 0x10
	strgt	r11, [r10, r9]		@ type 0x20
	b		main_loop
	
@@@@@@@@@@@@@@@@

@Type 0x30
@3XXXXXXX 000UYYYY
@30 = Code Type Or Byte
@XXXXXXX = Address
@           U = Bit-type Write, 0 for 8-Bit & 1 for 16-Bit
@            YYYY = 8/16-bit Value to increment by

@Type 0x38
@3XXXXXXX YYYYYYYY
@38 = Code Type Or Byte
@XXXXXXX = Address
@        YYYYYYYY = 32-bit Value to increment by
@NOTE: YYYYYYYY is signed so it can do increment and decrement

increment_write:
	cmp		r0, #0x38
	beq		increment_32
	
	bic		r10, r10, #0xF8000000
	and 	r1, r11, #0x00010000 	@Bit-type Load
	mov 	r1, r1, lsr #16 		@Bit-type Load
	cmp		r1, #0 					@Bit-type Load check
	ldreqb	r0, [r10]				@Load 8bits from address
	and		r11, #0x000000FF		@Cut off all but 8bits
	andne	r11, #0x0000FF00		@Cut off the bit check
	ldrneh	r0, [r10]				@Load 16bits from address
	
	add		r0, r0, r11				@Add value from [XXXXXXX] to increment/decrement value
	
	streqb	r0, [r10]				@Store 8bits back to address
	strneh	r0, [r10]				@Store 16bits back to address
	b 		main_loop

increment_32:
	bic		r10, r10, #0xF8000000
	ldr		r0, [r10]				@Load 32bits from address
	add		r0, r0, r11				@Add value from [XXXXXXX] to increment/decrement value
	str		r0, [r10]				@Store 32bits back to address
	b 		main_loop
	
@@@@@@@@@@@@@@@@

@Type 0x40

@4XXXXXXX TWWWZZZZ
@YYYYYYYY VVVVVVVV
@40 = Code Type Or Byte
@XXXXXXX = Address
@        T = Bit-type Write, 0 for 32-Bit, 1 for 16-Bit & 2 for 8-Bit
@         WWW = Number of times to repeat
@            ZZZZ = Increase Address by (Multiply by data size (1 << (2 - T)))
@YYYYYYYY = Start Value
@        VVVVVVVV = Increase Value by

slider_write:
	bic		r10, r10, #0xF8000000
	bic		r0, r11, #0xFF000000	@??ZZZZ
	bic		r0, r11, #0x00FF0000	@ZZZZ
	bic		r1, r11, #0xF0000000	@0WWW0000
	mov		r1, r1, lsr #16			@WWW
	mov		r2, r11, lsr #30		@T & 3
	ldmia	r12!, {r3, r4}			@ r3 - YYYYYYYY		r4 = VVVVVVVV
	mov		r5, #2
	sub		r5, r5, r2				@(2 - T)
	mov		r6, #1
	mov		r0, r0, lsl r5			@ZZZZ * (1 << (2 - T))
	
slider_loop:
	cmp 	r2, #1
	strlt 	r3, [r10],#4			@If 32bit
	streqh	r3, [r10],#2			@If 16bit
	strgtb	r3, [r10],#1			@If 8bit
	
	add		r3, r3, r4					@Increament start value by increase value
	
	subs	r1, #1
	bgt		slider_loop

	@If repeat amount == 0 then start parsing next code
	b 		main_loop

@@@@@@@@@@@@@@@@
@ type D0

@16-Bit
@DXXXXXXX ZZTUYYYY

@XXXXXXX = Pointer Address
@ZZ = Lines to skip, 00 means 01 by default 
@T = Condition type to check against VVVV [0 is ==, 1 is !=, 2 is <, 3 is>, 4 is == and == 0000, 4-7 follow the pattern set by 0-3]
@U = Bit-type Load, 0 for 16-Bit, and 1 for 8-Bit
@YYYY = Halfword/Byte to check against 

if_code:
	mov 	r1, r11, lsr #24 		@Lines to skip 
	and 	r2, r11, #0x00700000 	@Condition type
	mov 	r2, r2, lsr #20 		@Condition type
	and 	r3, r11, #0x00010000 	@Bit-type Load
	mov 	r3, r3, lsr #16 		@Bit-type Load
	bic		r10, r10, #0xF8000000 	@Pointer Address
	
	cmp		r3, #0 @Bit-type Load check
	ldreqh	r4, [r10] @Compare value 16bit
	biceq	r11, #0xFF000000
	biceq	r11, #0x00FF0000
	ldrneb	r4, [r10] @Compare value 8bit
	andne	r11, #0x000000FF
	
	cmp 	r2, #1
	blt		if_equals 				@If equal to YYYY
	beq		if_not_equal 			@If not equal to YYYY
	cmp 	r2, #3
	blt		if_less_than			@If lesser than YYYY
	beq		if_greater				@If greater than YYYY
	cmp		r2, #5
	blt		if_and_equal_zero		@If AND YYYY equal to 0000
	beq		if_and_not_equal_zero	@If AND YYYY not equal to 0000
	cmp		r2, #7
	blt		if_and_equal			@If AND YYYY equal to YYYY
	beq		if_and_not_equal		@If AND YYYY not equal to YYYY
	b 		main_loop				@return in case of invalid code
	
if_equals:
	cmp		r4, r11
	beq 	main_loop
	add		r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_not_equal:
	cmp		r4, r11
	bne 	main_loop
	add		r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_greater:
	cmp		r4, r11
	bgt 	main_loop
	add		r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_less_than:
	cmp		r4, r11
	blt 	main_loop
	add		r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_and_equal_zero:
	and		r4, r11
	cmp		r4, #0
	addne	r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_and_not_equal_zero:
	and		r4, r11
	cmp		r4, #0
	addeq	r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_and_equal:
	ands	r4, r11
	addne	r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop
	
if_and_not_equal:
	ands	r4, r11
	addeq	r12, r1, lsl #3 		@skip lines << 3
	b 		main_loop

@@@@@@@@@@@@@@@@

CB_return:
    ldmia r13!, {r0-r12, lr}
    bx lr

codeTableStart: .word 0xEFC0DE23
@codeTableEnd: .word 0xEFC0DE24

.end