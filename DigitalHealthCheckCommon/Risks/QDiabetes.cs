using System;

namespace QMSUK.QRisk
{
    /// <summary>
    /// Calculates the QDiabetes 10-year diabetes risk chance for a patient.
    /// </summary>
    public class QDiabetes
    {
        /// <summary>
        /// Calculates the 10-year diabetes risk for a female patient.
        /// </summary>
        /// <remarks>This chooses between Models A (no HbA1c) and C (HbA1c) based on whether HbA1c is null or not.</remarks>
        /// <param name="age">The patient's age.</param>
        /// <param name="atypicalAntipsy">if set to <c>true</c>, whether the patient takes atypical antipsychotics.</param>
        /// <param name="corticosteroids">if set to <c>true</c>, whether the patient takes corticosteroids..</param>
        /// <param name="cvd">if set to <c>true</c>, whether the patient has been diagnosed with CVD.</param>
        /// <param name="gestationalDiabetes">if set to <c>true</c>, whether the patient has gestational diabetes.</param>
        /// <param name="learningDifficulties">if set to <c>true</c>, whether the patient has any learning difficulties.</param>
        /// <param name="manicSchizophrenia">if set to <c>true</c>, whether the patient has manic schizophrenia.</param>
        /// <param name="polycysticOvaries">if set to <c>true</c>, whether the patient has polycystic ovaries.</param>
        /// <param name="statins">if set to <c>true</c>, whether the paitent takes statins.</param>
        /// <param name="treatedHypertension">if set to <c>true</c>, whether the patient has been treated for hypertension.</param>
        /// <param name="familyHistoryDiabetes">if set to <c>true</c>, whether the patient has a family history of diabetes.</param>
        /// <param name="hbA1c">The patient's blood sugar reading.</param>
        /// <param name="ethnicity">The patient's ethnicity.</param>
        /// <param name="smoking">The patient's smoking status.</param>
        /// <param name="town">The patient's townsend deprivation score.</param>
        /// <param name="bmi">The patient's body mass index.</param>
        /// <returns>The patients chance of developing diabetes in the next 10-years.</returns>
        public static double CalculateFemaleRisk
        (
            int age,
            bool atypicalAntipsy,
            bool corticosteroids,
            bool cvd,
            bool gestationalDiabetes,
            bool learningDifficulties,
            bool manicSchizophrenia,
            bool polycysticOvaries,
            bool statins,
            bool treatedHypertension,
            bool familyHistoryDiabetes,
            double? hbA1c,
            Ethnicity ethnicity,
            Smoking smoking,
            double? town,
            double bmi
        ) => hbA1c.HasValue ? type2_female_raw
        (
            age,
            atypicalAntipsy ? 1 : 0,
            corticosteroids ? 1 : 0,
            cvd ? 1 : 0,
            gestationalDiabetes ? 1 : 0,
            learningDifficulties ? 1 : 0,
            manicSchizophrenia ? 1 : 0,
            polycysticOvaries ? 1 : 0,
            statins ? 1 : 0,
            treatedHypertension ? 1 : 0,
            bmi,
            EthnicityGroups.GroupingFor(ethnicity),
            familyHistoryDiabetes ? 1 : 0,
            hbA1c.Value,
            (int)smoking,
            town ?? 0d
        ) : type2_female_raw_no_hba1c
        (
            age,
            atypicalAntipsy ? 1 : 0,
            corticosteroids ? 1 : 0,
            cvd ? 1 : 0,
            gestationalDiabetes ? 1 : 0,
            learningDifficulties ? 1 : 0,
            manicSchizophrenia ? 1 : 0,
            polycysticOvaries ? 1 : 0,
            statins ? 1 : 0,
            treatedHypertension ? 1 : 0,
            bmi,
            EthnicityGroups.GroupingFor(ethnicity),
            familyHistoryDiabetes ? 1 : 0,
            (int)smoking,
            town ?? 0d
        );

        /// <summary>
        /// Calculates the 10-year diabetes risk for a male patient.
        /// </summary>
        /// <remarks>This chooses between Models A (no HbA1c) and C (HbA1c) based on whether HbA1c is null or not.</remarks>
        /// <param name="age">The patient's age.</param>
        /// <param name="atypicalAntipsy">if set to <c>true</c>, whether the patient takes atypical antipsychotics.</param>
        /// <param name="corticosteroids">if set to <c>true</c>, whether the patient takes corticosteroids..</param>
        /// <param name="cvd">if set to <c>true</c>, whether the patient has been diagnosed with CVD.</param>
        /// <param name="learningDifficulties">if set to <c>true</c>, whether the patient has any learning difficulties.</param>
        /// <param name="manicSchizophrenia">if set to <c>true</c>, whether the patient has manic schizophrenia.</param>
        /// <param name="statins">if set to <c>true</c>, whether the paitent takes statins.</param>
        /// <param name="treatedHypertension">if set to <c>true</c>, whether the patient has been treated for hypertension.</param>
        /// <param name="familyHistoryDiabetes">if set to <c>true</c>, whether the patient has a family history of diabetes.</param>
        /// <param name="hbA1c">The patient's blood sugar reading.</param>
        /// <param name="ethnicity">The patient's ethnicity.</param>
        /// <param name="smoking">The patient's smoking status.</param>
        /// <param name="town">The patient's townsend deprivation score.</param>
        /// <param name="bmi">The patient's body mass index.</param>
        /// <returns>The patients chance of developing diabetes in the next 10-years.</returns>
        public static double CalculateMaleRisk(
            int age,
            bool atypicalAntipsy,
            bool corticosteroids,
            bool cvd,
            bool learningDifficulties,
            bool manicSchizophrenia,
            bool statins,
            bool treatedHypertension,
            bool familyHistoryDiabetes,
            double? hbA1c,
            Ethnicity ethnicity,
            Smoking smoking,
            double? town,
            double bmi) => hbA1c.HasValue ? type2_male_raw
            (
                age,
                atypicalAntipsy ? 1 : 0,
                corticosteroids ? 1 : 0,
                cvd ? 1 : 0,
                learningDifficulties ? 1 : 0,
                manicSchizophrenia ? 1 : 0,
                statins ? 1 : 0,
                treatedHypertension ? 1 : 0,
                bmi,
                EthnicityGroups.GroupingFor(ethnicity),
                familyHistoryDiabetes ? 1 : 0,
                hbA1c.Value,
                (int)smoking,
                town ?? 0d
            ) :
            type2_male_raw_no_hba1c
            (
                age,
                atypicalAntipsy ? 1 : 0,
                corticosteroids ? 1 : 0,
                cvd ? 1 : 0,
                learningDifficulties ? 1 : 0,
                manicSchizophrenia ? 1 : 0,
                statins ? 1 : 0,
                treatedHypertension ? 1 : 0,
                bmi,
                EthnicityGroups.GroupingFor(ethnicity),
                familyHistoryDiabetes ? 1 : 0,
                (int)smoking,
                town ?? 0d
            ) ;

        // Model A for when HbA1c is unknown

        /* 
         * Copyright 2017 ClinRisk Ltd. 
         * 
         * This file is part of QDiabetes-2018 (https://qdiabetes.org).
         * 
         * QDiabetes-2018 is free software: you can redistribute it and/or modify
         * it under the terms of the GNU Affero General Public License as published by
         * the Free Software Foundation, either version 3 of the License, or
         * (at your option) any later version.
         * 
         * QDiabetes-2018 is distributed in the hope that it will be useful,
         * but WITHOUT ANY WARRANTY; without even the implied warranty of
         * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
         * GNU Affero General Public License for more details.
         * 
         * You should have received a copy of the GNU Affero General Public License
         * along with QDiabetes-2018.  If not, see http://www.gnu.org/licenses/.
         * 
         * Additional terms
         * 
         * The following disclaimer must be held together with any risk score score generated by this code.  
         * If the score is displayed, then this disclaimer must be displayed or otherwise be made easily accessible, e.g. by a prominent link alongside it.
         *   The initial version of this file, to be found at http://qdiabetes.org, faithfully implements QDiabetes-2018.
         *   ClinRisk Ltd. have released this code under the GNU Affero General Public License to enable others to implement the algorithm faithfully.
         *   However, the nature of the GNU Affero General Public License is such that we cannot prevent, for example, someone accidentally 
         *   altering the coefficients, getting the inputs wrong, or just poor programming.
         *   ClinRisk Ltd. stress, therefore, that it is the responsibility of the end user to check that the source that they receive produces the same 
         *   results as the original code found at http://qdiabetes.org.
         *   Inaccurate implementations of risk scores can lead to wrong patients being given the wrong treatment.
         * 
         * End of additional terms
         *
         */
        static double type2_female_raw_no_hba1c(
            int age,
            int b_atypicalantipsy,
            int b_corticosteroids,
            int b_cvd,
            int b_gestdiab,
            int b_learning,
            int b_manicschiz,
            int b_pos,
            int b_statin,
            int b_treatedhyp,
            double bmi,
            int ethrisk,
            int fh_diab,
            int smoke_cat,
            double town)
        {
            var surv = 10;
            var survivor = new []{
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0.986227273941040
            };

            /* The conditional arrays */

            var Iethrisk = new [] {
                0,
                1.0695857881565456000000000,
                1.3430172097414006000000000,
                1.8029022579794518000000000,
                1.1274654517708020000000000,
                0.4214631490239910100000000,
                0.2850919645908353000000000,
                0.8815108797589199500000000,
                0.3660573343168487300000000
            };

            var Ismoke = new [] {
                0,
                0.0656016901750590550000000,
                0.2845098867369837400000000,
                0.3567664381700702000000000,
                0.5359517110678775300000000
            };

            /* Applying the fractional polynomial transforms */
            /* (which includes scaling)                      */

            double dage = age;
            dage=dage/10;
            double age_2 = Math.Pow(dage,3);
            double age_1 = Math.Pow(dage,.5);
            double dbmi = bmi;
            dbmi=dbmi/10;
            double bmi_1 = dbmi;
            double bmi_2 = Math.Pow(dbmi,3);

            /* Centring the continuous variables */

            age_1 = age_1 - 2.123332023620606;
            age_2 = age_2 - 91.644744873046875;
            bmi_1 = bmi_1 - 2.571253299713135;
            bmi_2 = bmi_2 - 16.999439239501953;
            town = town - 0.391116052865982;

            /* Start of Sum */
            double a=0;

            /* The conditional sums */

            a += Iethrisk[ethrisk];
            a += Ismoke[smoke_cat];

            /* Sum from continuous values */

            a += age_1 * 4.3400852699139278000000000;
            a += age_2 * -0.0048771702696158879000000;
            a += bmi_1 * 2.9320361259524925000000000;
            a += bmi_2 * -0.0474002058748434900000000;
            a += town * 0.0373405696180491510000000;

            /* Sum from boolean values */

            a += b_atypicalantipsy * 0.5526764611098438100000000;
            a += b_corticosteroids * 0.2679223368067459900000000;
            a += b_cvd * 0.1779722905458669100000000;
            a += b_gestdiab * 1.5248871531467574000000000;
            a += b_learning * 0.2783514358717271700000000;
            a += b_manicschiz * 0.2618085210917905900000000;
            a += b_pos * 0.3406173988206666100000000;
            a += b_statin * 0.6590728773280821700000000;
            a += b_treatedhyp * 0.4394758285813711900000000;
            a += fh_diab * 0.5313359456558733900000000;

            /* Sum from interaction terms */

            a += age_1 * b_atypicalantipsy * -0.8031518398316395100000000;
            a += age_1 * b_learning * -0.8641596002882057100000000;
            a += age_1 * b_statin * -1.9757776696583935000000000;
            a += age_1 * bmi_1 * 0.6553138757562945200000000;
            a += age_1 * bmi_2 * -0.0362096572016301770000000;
            a += age_1 * fh_diab * -0.2641171450558896200000000;
            a += age_2 * b_atypicalantipsy * 0.0004684041181021049800000;
            a += age_2 * b_learning * 0.0006724968808953360200000;
            a += age_2 * b_statin * 0.0023750534194347966000000;
            a += age_2 * bmi_1 * -0.0044719662445263054000000;
            a += age_2 * bmi_2 * 0.0001185479967753342000000;
            a += age_2 * fh_diab * 0.0004161025828904768300000;

            /* Calculate the score itself */
            double score = 100.0 * (1 - Math.Pow(survivor[surv], Math.Exp(a)) );
            return score;
        }

        /* 
         * Copyright 2017 ClinRisk Ltd. 
         * 
         * This file is part of QDiabetes-2018 (https://qdiabetes.org).
         * 
         * QDiabetes-2018 is free software: you can redistribute it and/or modify
         * it under the terms of the GNU Affero General Public License as published by
         * the Free Software Foundation, either version 3 of the License, or
         * (at your option) any later version.
         * 
         * QDiabetes-2018 is distributed in the hope that it will be useful,
         * but WITHOUT ANY WARRANTY; without even the implied warranty of
         * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
         * GNU Affero General Public License for more details.
         * 
         * You should have received a copy of the GNU Affero General Public License
         * along with QDiabetes-2018.  If not, see http://www.gnu.org/licenses/.
         * 
         * Additional terms
         * 
         * The following disclaimer must be held together with any risk score score generated by this code.  
         * If the score is displayed, then this disclaimer must be displayed or otherwise be made easily accessible, e.g. by a prominent link alongside it.
         *   The initial version of this file, to be found at http://qdiabetes.org, faithfully implements QDiabetes-2018.
         *   ClinRisk Ltd. have released this code under the GNU Affero General Public License to enable others to implement the algorithm faithfully.
         *   However, the nature of the GNU Affero General Public License is such that we cannot prevent, for example, someone accidentally 
         *   altering the coefficients, getting the inputs wrong, or just poor programming.
         *   ClinRisk Ltd. stress, therefore, that it is the responsibility of the end user to check that the source that they receive produces the same 
         *   results as the original code found at http://qdiabetes.org.
         *   Inaccurate implementations of risk scores can lead to wrong patients being given the wrong treatment.
         * 
         * End of additional terms
         *
         */

        // Model C

        static double type2_female_raw(
            int age,
            int b_atypicalantipsy,
            int b_corticosteroids,
            int b_cvd,
            int b_gestdiab,
            int b_learning,
            int b_manicschiz,
            int b_pos,
            int b_statin,
            int b_treatedhyp,
            double bmi,
            int ethrisk,
            int fh_diab,
            double hba1c,
            int smoke_cat,
            double town)
        {
            var surv = 10;
            var survivor = new [] {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0.988788545131683
            };

            /* The conditional arrays */

            var Iethrisk = new [] {
                0,
                0.5990951599291540800000000,
                0.7832030965635389300000000,
                1.1947351247960103000000000,
                0.7141744699168143300000000,
                0.1195328468388768800000000,
                0.0136688728784904270000000,
                0.5709226537693945500000000,
                0.1709107628106929200000000
            };
            var Ismoke = new [] {
                0,
                0.0658482585100006730000000,
                0.1458413689734224000000000,
                0.1525864247480118700000000,
                0.3078741679661397600000000
            };

            /* Applying the fractional polynomial transforms */
            /* (which includes scaling)                      */

            double dage = age;
            dage = dage / 10;
            double age_1 = Math.Pow(dage, .5);
            double age_2 = Math.Pow(dage, 3);
            double dbmi = bmi;
            dbmi = dbmi / 10;
            double bmi_2 = Math.Pow(dbmi, 3);
            double bmi_1 = dbmi;
            double dhba1c = hba1c;
            dhba1c = dhba1c / 10;
            double hba1c_1 = Math.Pow(dhba1c, .5);
            double hba1c_2 = dhba1c;

            /* Centring the continuous variables */

            age_1 = age_1 - 2.123332023620606;
            age_2 = age_2 - 91.644744873046875;
            bmi_1 = bmi_1 - 2.571253299713135;
            bmi_2 = bmi_2 - 16.999439239501953;
            hba1c_1 = hba1c_1 - 1.886751174926758;
            hba1c_2 = hba1c_2 - 3.559829950332642;
            town = town - 0.391116052865982;

            /* Start of Sum */
            double a = 0;

            /* The conditional sums */

            a += Iethrisk[ethrisk];
            a += Ismoke[smoke_cat];

            /* Sum from continuous values */

            a += age_1 * 3.5655214891947722000000000;
            a += age_2 * -0.0056158243572733135000000;
            a += bmi_1 * 2.5043028874544841000000000;
            a += bmi_2 * -0.0428758018926904610000000;
            a += hba1c_1 * 8.7368031307362184000000000;
            a += hba1c_2 * -0.0782313866699499700000000;
            a += town * 0.0358668220563482940000000;

            /* Sum from boolean values */

            a += b_atypicalantipsy * 0.5497633311042200400000000;
            a += b_corticosteroids * 0.1687220550638970400000000;
            a += b_cvd * 0.1644330036273934400000000;
            a += b_gestdiab * 1.1250098105171140000000000;
            a += b_learning * 0.2891205831073965800000000;
            a += b_manicschiz * 0.3182512249068407700000000;
            a += b_pos * 0.3380644414098174500000000;
            a += b_statin * 0.4559396847381116400000000;
            a += b_treatedhyp * 0.4040022295023758000000000;
            a += fh_diab * 0.4428015404826031700000000;

            /* Sum from interaction terms */

            a += age_1 * b_atypicalantipsy * -0.8125434197162131300000000;
            a += age_1 * b_learning * -0.9084665765269808200000000;
            a += age_1 * b_statin * -1.8557960585560658000000000;
            a += age_1 * bmi_1 * 0.6023218765235252000000000;
            a += age_1 * bmi_2 * -0.0344950383968044700000000;
            a += age_1 * fh_diab * -0.2727571351506187200000000;
            a += age_1 * hba1c_1 * 25.4412033227367150000000000;
            a += age_1 * hba1c_2 * -6.8076080421556107000000000;
            a += age_2 * b_atypicalantipsy * 0.0004665611306005428000000;
            a += age_2 * b_learning * 0.0008518980139928006500000;
            a += age_2 * b_statin * 0.0022627250963352537000000;
            a += age_2 * bmi_1 * -0.0043386645663133425000000;
            a += age_2 * bmi_2 * 0.0001162778561671208900000;
            a += age_2 * fh_diab * 0.0004354519795220774900000;
            a += age_2 * hba1c_1 * -0.0522541355885925220000000;
            a += age_2 * hba1c_2 * 0.0140548259061144530000000;

            /* Calculate the score itself */
            double score = 100.0 * (1 - Math.Pow(survivor[surv], Math.Exp(a)));
            return score;
        }

        /* 
        * Copyright 2017 ClinRisk Ltd. 
        * 
        * This file is part of QDiabetes-2018 (https://qdiabetes.org).
        * 
        * QDiabetes-2018 is free software: you can redistribute it and/or modify
        * it under the terms of the GNU Affero General Public License as published by
        * the Free Software Foundation, either version 3 of the License, or
        * (at your option) any later version.
        * 
        * QDiabetes-2018 is distributed in the hope that it will be useful,
        * but WITHOUT ANY WARRANTY; without even the implied warranty of
        * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        * GNU Affero General Public License for more details.
        * 
        * You should have received a copy of the GNU Affero General Public License
        * along with QDiabetes-2018.  If not, see http://www.gnu.org/licenses/.
        * 
        * Additional terms
        * 
        * The following disclaimer must be held together with any risk score score generated by this code.  
        * If the score is displayed, then this disclaimer must be displayed or otherwise be made easily accessible, e.g. by a prominent link alongside it.
        *   The initial version of this file, to be found at http://qdiabetes.org, faithfully implements QDiabetes-2018.
        *   ClinRisk Ltd. have released this code under the GNU Affero General Public License to enable others to implement the algorithm faithfully.
        *   However, the nature of the GNU Affero General Public License is such that we cannot prevent, for example, someone accidentally 
        *   altering the coefficients, getting the inputs wrong, or just poor programming.
        *   ClinRisk Ltd. stress, therefore, that it is the responsibility of the end user to check that the source that they receive produces the same 
        *   results as the original code found at http://qdiabetes.org.
        *   Inaccurate implementations of risk scores can lead to wrong patients being given the wrong treatment.
        * 
        * End of additional terms
        *
        */

        // Model A - unknown hbA1c

        static double type2_male_raw_no_hba1c(
            int age,
            int b_atypicalantipsy,
            int b_corticosteroids,
            int b_cvd,
            int b_learning,
            int b_manicschiz,
            int b_statin,
            int b_treatedhyp,
            double bmi,
            int ethrisk,
            int fh_diab,
            int smoke_cat,
            double town)
        {
            var surv = 10;
            var survivor = new [] {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0.978732228279114
            };

            /* The conditional arrays */

            var Iethrisk = new [] {
                0,
                1.1000230829124793000000000,
                1.2903840126147210000000000,
                1.6740908848727458000000000,
                1.1400446789147816000000000,
                0.4682468169065580600000000,
                0.6990564996301544800000000,
                0.6894365712711156800000000,
                0.4172222846773820900000000
            };

            var Ismoke = new [] {
                0,
                0.1638740910548557300000000,
                0.3185144911395897900000000,
                0.3220726656778343200000000,
                0.4505243716340953100000000
            };

            /* Applying the fractional polynomial transforms */
            /* (which includes scaling)                      */

            double dage = age;
            dage=dage/10;
            double age_2 = Math.Pow(dage,3);
            double age_1 = Math.Log(dage);
            double dbmi = bmi;
            dbmi=dbmi/10;
            double bmi_2 = Math.Pow(dbmi,3);
            double bmi_1 = Math.Pow(dbmi,2);

            /* Centring the continuous variables */

            age_1 = age_1 - 1.496392488479614;
            age_2 = age_2 - 89.048171997070313;
            bmi_1 = bmi_1 - 6.817805767059326;
            bmi_2 = bmi_2 - 17.801923751831055;
            town = town - 0.515986680984497;

            /* Start of Sum */
            double a=0;

            /* The conditional sums */

            a += Iethrisk[ethrisk];
            a += Ismoke[smoke_cat];

            /* Sum from continuous values */

            a += age_1 * 4.4642324388691348000000000;
            a += age_2 * -0.0040750108019255568000000;
            a += bmi_1 * 0.9512902786712067500000000;
            a += bmi_2 * -0.1435248827788547500000000;
            a += town * 0.0259181820676787250000000;

            /* Sum from boolean values */

            a += b_atypicalantipsy * 0.4210109234600543600000000;
            a += b_corticosteroids * 0.2218358093292538400000000;
            a += b_cvd * 0.2026960575629002100000000;
            a += b_learning * 0.2331532140798696100000000;
            a += b_manicschiz * 0.2277044952051772700000000;
            a += b_statin * 0.5849007543114134200000000;
            a += b_treatedhyp * 0.3337939218350107800000000;
            a += fh_diab * 0.6479928489936953600000000;

            /* Sum from interaction terms */

            a += age_1 * b_atypicalantipsy * -0.9463772226853415200000000;
            a += age_1 * b_learning * -0.9384237552649983300000000;
            a += age_1 * b_statin * -1.7479070653003299000000000;
            a += age_1 * bmi_1 * 0.4514759924187976600000000;
            a += age_1 * bmi_2 * -0.1079548126277638100000000;
            a += age_1 * fh_diab * -0.6011853042930119800000000;
            a += age_2 * b_atypicalantipsy * -0.0000519927442172335000000;
            a += age_2 * b_learning * 0.0007102643855968814100000;
            a += age_2 * b_statin * 0.0013508364599531669000000;
            a += age_2 * bmi_1 * -0.0011797722394560309000000;
            a += age_2 * bmi_2 * 0.0002147150913931929100000;
            a += age_2 * fh_diab * 0.0004914185594087803400000;

            /* Calculate the score itself */
            double score = 100.0 * (1 - Math.Pow(survivor[surv], Math.Exp(a)) );
            return score;
        }

        /* 
         * Copyright 2017 ClinRisk Ltd. 
         * 
         * This file is part of QDiabetes-2018 (https://qdiabetes.org).
         * 
         * QDiabetes-2018 is free software: you can redistribute it and/or modify
         * it under the terms of the GNU Affero General Public License as published by
         * the Free Software Foundation, either version 3 of the License, or
         * (at your option) any later version.
         * 
         * QDiabetes-2018 is distributed in the hope that it will be useful,
         * but WITHOUT ANY WARRANTY; without even the implied warranty of
         * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
         * GNU Affero General Public License for more details.
         * 
         * You should have received a copy of the GNU Affero General Public License
         * along with QDiabetes-2018.  If not, see http://www.gnu.org/licenses/.
         * 
         * Additional terms
         * 
         * The following disclaimer must be held together with any risk score score generated by this code.  
         * If the score is displayed, then this disclaimer must be displayed or otherwise be made easily accessible, e.g. by a prominent link alongside it.
         *   The initial version of this file, to be found at http://qdiabetes.org, faithfully implements QDiabetes-2018.
         *   ClinRisk Ltd. have released this code under the GNU Affero General Public License to enable others to implement the algorithm faithfully.
         *   However, the nature of the GNU Affero General Public License is such that we cannot prevent, for example, someone accidentally 
         *   altering the coefficients, getting the inputs wrong, or just poor programming.
         *   ClinRisk Ltd. stress, therefore, that it is the responsibility of the end user to check that the source that they receive produces the same 
         *   results as the original code found at http://qdiabetes.org.
         *   Inaccurate implementations of risk scores can lead to wrong patients being given the wrong treatment.
         * 
         * End of additional terms
         *
         */

        // Model C

        static double type2_male_raw(
            int age,
            int b_atypicalantipsy,
            int b_corticosteroids,
            int b_cvd,
            int b_learning,
            int b_manicschiz,
            int b_statin,
            int b_treatedhyp,
            double bmi,
            int ethrisk,
            int fh_diab,
            double hba1c,
            int smoke_cat,
            double town)
        {
            var surv = 10;
            var survivor = new [] {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0.981181740760803
            };

            /* The conditional arrays */

            var Iethrisk = new [] {
                0,
                0.6757120705498780300000000,
                0.8314732504966345600000000,
                1.0969133802228563000000000,
                0.7682244636456048200000000,
                0.2089752925910850200000000,
                0.3809159378197057900000000,
                0.3423583679661269500000000,
                0.2204647785343308300000000
            };
            var Ismoke = new [] {
                0,
                0.1159289120687865100000000,
                0.1462418263763327100000000,
                0.1078142411249314200000000,
                0.1984862916366847400000000
            };

            /* Applying the fractional polynomial transforms */
            /* (which includes scaling)                      */

            double dage = age;
            dage = dage / 10;
            double age_1 = Math.Log(dage);
            double age_2 = Math.Pow(dage, 3);
            double dbmi = bmi;
            dbmi = dbmi / 10;
            double bmi_1 = Math.Pow(dbmi, 2);
            double bmi_2 = Math.Pow(dbmi, 3);
            double dhba1c = hba1c;
            dhba1c = dhba1c / 10;
            double hba1c_1 = Math.Pow(dhba1c, .5);
            double hba1c_2 = dhba1c;

            /* Centring the continuous variables */

            age_1 = age_1 - 1.496392488479614;
            age_2 = age_2 - 89.048171997070313;
            bmi_1 = bmi_1 - 6.817805767059326;
            bmi_2 = bmi_2 - 17.801923751831055;
            hba1c_1 = hba1c_1 - 1.900265336036682;
            hba1c_2 = hba1c_2 - 3.611008167266846;
            town = town - 0.515986680984497;

            /* Start of Sum */
            double a = 0;

            /* The conditional sums */

            a += Iethrisk[ethrisk];
            a += Ismoke[smoke_cat];

            /* Sum from continuous values */

            a += age_1 * 4.0193435623978031000000000;
            a += age_2 * -0.0048396442306278238000000;
            a += bmi_1 * 0.8182916890534932500000000;
            a += bmi_2 * -0.1255880870135964200000000;
            a += hba1c_1 * 8.0511642238857934000000000;
            a += hba1c_2 * -0.1465234689391449500000000;
            a += town * 0.0252299651849007270000000;

            /* Sum from boolean values */

            a += b_atypicalantipsy * 0.4554152522017330100000000;
            a += b_corticosteroids * 0.1381618768682392200000000;
            a += b_cvd * 0.1454698889623951800000000;
            a += b_learning * 0.2596046658040857000000000;
            a += b_manicschiz * 0.2852378849058589400000000;
            a += b_statin * 0.4255195190118552500000000;
            a += b_treatedhyp * 0.3316943000645931100000000;
            a += fh_diab * 0.5661232594368061900000000;

            /* Sum from interaction terms */

            a += age_1 * b_atypicalantipsy * -1.0013331909079835000000000;
            a += age_1 * b_learning * -0.8916465737221592700000000;
            a += age_1 * b_statin * -1.7074561167819817000000000;
            a += age_1 * bmi_1 * 0.4507452747267244300000000;
            a += age_1 * bmi_2 * -0.1085185980916560100000000;
            a += age_1 * fh_diab * -0.6141009388709716100000000;
            a += age_1 * hba1c_1 * 27.6705938271465650000000000;
            a += age_1 * hba1c_2 * -7.4006134846785434000000000;
            a += age_2 * b_atypicalantipsy * 0.0002245597398574240700000;
            a += age_2 * b_learning * 0.0006604436076569648200000;
            a += age_2 * b_statin * 0.0013873509357389619000000;
            a += age_2 * bmi_1 * -0.0012224736160287865000000;
            a += age_2 * bmi_2 * 0.0002266731010346126000000;
            a += age_2 * fh_diab * 0.0005060258289477209100000;
            a += age_2 * hba1c_1 * -0.0592014581247543300000000;
            a += age_2 * hba1c_2 * 0.0155920894851499880000000;

            /* Calculate the score itself */
            double score = 100.0 * (1 - Math.Pow(survivor[surv], Math.Exp(a)));
            return score;
        }
    }
}