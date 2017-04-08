package com.vz.monitor.util;

public class NumberUtil
{

    /**
     * 将byte数据转换成int, byte是1个字节，int是4个字节
     * 注意：byte[4] 放在了int 的低字节上， (大端字节序，高字节存于内存低地址，低字节存于内存高地址) big endian
     * @param b
     * @return
     */
    public static final int byte2Int(byte b[])
    {
        if (null == b || b.length < 4)
        {
            return 0;
        }
        return b[3] & 0xff | (b[2] & 0xff) << 8 | (b[1] & 0xff) << 16
                | (b[0] & 0xff) << 24;
    }

    /**
     *  将byte数据转换成int, byte是1个字节，int是4个字节
     * 注意：byte[0] 放在了int 的低字节上 ,小端 litte endian
     * @param b
     * @return
     */
    public static final int leByte2Int(byte[] b)
    {
        if (null == b || b.length < 4)
        {
            return 0;
        }
        return b[0] & 0xff | (b[1] & 0xff) << 8 | (b[2] & 0xff) << 16
                | (b[3] & 0xff) << 24;
    }

    /**
     * 将int以大端序放到byte上
     * @param intValue
     * @return
     */
    public static final byte[] int2Byte(int intValue)
    {
        byte[] b = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            b[i] = (byte) (intValue >> 8 * (3 - i) & 0xFF);
        }
        return b;
    }

    /**
     * 将int以小端序放到byte上
     * @param value
     * @return
     */
    public static final byte[] int2LeByte(int value)
    {
        byte[] b = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            b[i] = (byte) (value >> 8 * i & 0xFF);
        }
        return b;
    }

    /**
     * 遇到浮点型数据，限制小数位
     * @param num
     * @return
     */
    public static final String limitOneDecimal(float num)
    {
        java.text.DecimalFormat df = new java.text.DecimalFormat("#.0");
        return df.format(num);
    }
}
