package tt;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

import org.apache.commons.io.FileUtils;

/**
 * 
 * @filename:	 com.talent.platform.demo.tool.JsFilesToOne
 * @copyright:   Copyright (c)2010
 * @company:     talent
 * @author:      谭耀武
 * @version:     1.0
 * @create time: 2010-4-15 上午11:11:03
 * @record
 * <table cellPadding="3" cellSpacing="0" style="width:600px">
 * <thead style="font-weight:bold;background-color:#e3e197">
 * 	<tr>   <td>date</td>	<td>author</td>		<td>version</td>	<td>description</td></tr>
 * </thead>
 * <tbody style="background-color:#ffffeb">
 * 	<tr><td>2010-4-15</td>	<td>谭耀武</td>	<td>1.0</td>	<td>create</td></tr>
 * </tbody>
 * </table>
 */
public class JsFilesToOne {

	public static void toOne(String dirPath, String newFileName) throws Exception {
		String encoding = "utf-8";
		java.util.Properties p = new Properties();
		p.load(new FileInputStream(new File(dirPath + "/config.properties")));
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < p.size(); i++) {
			p.getProperty("" + i);
			sb.append(FileUtils.readFileToString(new File(dirPath + "/" + p.getProperty("" + i)), encoding));
			sb.append("\r\n");
		}
		FileUtils.writeStringToFile(new File(dirPath + "/" + newFileName), sb.toString(), encoding);
	}

	public static void modifyFileName(String dirPath, String fileNameSuffix) throws Exception {
		File dir = new File(dirPath);
		File[] files = dir.listFiles();
		for (int i = 0; i < files.length; i++) {
			files[i].renameTo(new File(files[i].getParentFile().getAbsolutePath() + File.separator + fileNameSuffix
					+ files[i].getName()));
		}
	}

	public static void main(String[] args) throws Exception {

		String basePath = "./";
		fileToOne(basePath);

//		try {
//			setupTo("D:/work/newsvn/frontend/branches/v_1.7.0_20150604/nginx/html");
//
//		} catch (Exception e) {
//			e.printStackTrace();
//		}
		
		
		


	}

	private static void setupTo(String webappPath) throws IOException {
		File jsFile = new File("./js/validate/talent-validate-all.js");
		File cssFile = new File("./js/validate/css/validate.css");
//		File imgDir = new File("./js/validate/css/img");

		File webappFileDir = new File(new File(webappPath), "js/validate/css/");
		webappFileDir.mkdirs();

		FileUtils.copyFile(jsFile, new File(new File(webappPath), "js/validate/talent-validate-all.js"));
		FileUtils.copyFile(cssFile, new File(new File(webappPath), "js/validate/css/validate.css"));
//		FileUtils.copyDirectory(imgDir, new File(new File(webappPath), "js/validate/css/img"));

		System.out.println("talent-validate installed to " + webappPath);

		//<link  href='<c:url value='/js/validate/css/validate.css' />' type='text/css' rel='stylesheet' />
		//<script src='<c:url value='/js/validate/talent-validate-all.js' />' language='javascript'></script>

		String tempString = "<link  href='<c:url value=\"/js/validate/css/validate.css\" />' type='text/css' rel='stylesheet' />  \r\n";
		tempString += "<script src='<c:url value=\"/js/validate/talent-validate-all.js\" />' language='javascript'></script>  \r\n";

		System.out.println("you can imported it as: --------------------------- ");
		System.out.println(tempString);

		System.out.println("\r\n\r\n");

		//<link type='text/css' rel='stylesheet' href='../js/validate/css/validate.css' /> 
		//<script src='../js/validate/talent-validate-all.js' language='javascript'>
	}

	private static void fileToOne(String bastPath) throws Exception {
		//F:/work/talent-vaidates/talent-validate-2.1.0/js
		String[] files = new String[100];
		int i = 0;
		//String bastPath = "F:/work/talent-vaidates/talent-validate-2.1.0";
		//F:/work/talent-vaidates/talent-validate-2.1.0
		files[i++] = bastPath + "/js/validate/i18n/message_zh_CN.js";
		files[i++] = bastPath + "/js/validate/utils/core.js";

		files[i++] = bastPath + "/js/validate/config/config.js";

		files[i++] = bastPath + "/js/validate/utils/utils.js";
		files[i++] = bastPath + "/js/validate/utils/api.js";

		files[i++] = bastPath + "/js/validate/handle/bh.js";
		files[i++] = bastPath + "/js/validate/handle/error/text.js";
		files[i++] = bastPath + "/js/validate/handle/error/alert.js";
		files[i++] = bastPath + "/js/validate/handle/tip/tip.js";

		files[i++] = bastPath + "/js/validate/validator/BaseValidator.js";
		files[i++] = bastPath + "/js/validate/validator/Field.js";

		files[i++] = bastPath + "/js/validate/filter/DefaultFilter.js";
		files[i++] = bastPath + "/js/validate/filter/FormFilter.js";
		files[i++] = bastPath + "/js/validate/filter/ElementFilter.js";
		files[i++] = bastPath + "/js/validate/filter/IdFilter.js";
		files[i++] = bastPath + "/js/validate/filter/NameFilter.js";

		files[i++] = bastPath + "/js/validate/validator/RegexValidator.js";
		files[i++] = bastPath + "/js/validate/validator/RequiredValidator.js";
		//files[i++] = bastPath + "/js/validate/validator/EmailValidator.js";
		files[i++] = bastPath + "/js/validate/validator/DatetimeValidator.js";
		files[i++] = bastPath + "/js/validate/validator/NumValidator.js";
		files[i++] = bastPath + "/js/validate/validator/IntValidator.js";
		files[i++] = bastPath + "/js/validate/validator/NumRangeValidator.js";
		files[i++] = bastPath + "/js/validate/validator/LengthValidator.js";
		files[i++] = bastPath + "/js/validate/validator/SelectCountValidator.js";

		files[i++] = bastPath + "/js/validate/validator/OnlyShow.js";
		files[i++] = bastPath + "/js/validate/validator/RemoteValidator.js";

		files[i++] = bastPath + "/js/validate/validator/CompareValidator.js";
		files[i++] = bastPath + "/js/validate/validator/GroupValidator.js";

		files[i++] = bastPath + "/js/validate/validator/ExpValidator.js";

		//files[i++] = bastPath + "/js/validate/validator/IpValidator.js";

		files[i++] = bastPath + "/js/validate/validator/ValidatorFactory.js";

		String encoding = "utf-8";
		StringBuilder sb = new StringBuilder();
		for (int j = 0; j < files.length; j++) {
			if (files[j] != null) {
				try {
					sb.append(FileUtils.readFileToString(new File(files[j]), encoding));
					sb.append("\r\n");
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		}
//		File f = new File(bastPath + "/js/validate/talent-validate-all.js");
		File f2 = new File(bastPath + "/js/validate/talent-validate-all.js");
//		FileUtils.writeStringToFile(f, sb.toString(), encoding);
		FileUtils.writeStringToFile(f2, sb.toString(), encoding);
		//		FileUtils.writeStringToFile(new File("E:/tech/spring/SpringSource-spring-mvc-showcase-b1c4b3d/src/main/webapp/resources/validate/talent-validate-all-min.js"), sb.toString(), encoding);
	}
}
