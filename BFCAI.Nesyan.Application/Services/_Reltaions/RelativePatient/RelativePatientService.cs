using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders.Medications;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Specifications.PatientRelatives;
using BFCAI.Nesyan.Domain.Specifications.Patients;
using BFCAI.Nesyan.Domain.Specifications.Relatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services._Reltaions.RelativePatient
{
    public class RelativePatientService(IUnitOfWork unitOfWork, IMapper mapper) : IRelativePatientService
    {
        public async Task<PatientSummaryV2Dto> RelativeSearchByUserName(string userName)
        {
            var specs =new PatientSerachSpecifications(userName);
            var patient =await unitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if(patient == null)
                throw new NotFoundException(nameof(patient),userName);
            return mapper.Map<PatientSummaryV2Dto>(patient);
        }
        public async Task AddExistingPatient(int relativeId, VerifyPatientDto dto)
        {
            var specs = new PatientSerachSpecifications(dto.NationalId, dto.Email);
            var patient = await unitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if (patient == null)
                throw new BadRequestException("Data Incorrect");
            var repo = unitOfWork.GetRepository<PatientRelative, int>();
            await repo.AddAsync(new PatientRelative { RelativeId = relativeId, PatientId = patient.Id });
            await unitOfWork.CompleteAsync();
        }

        public async Task CreateRelativePatientRelation(int relativeId,int patientId)
        {
            var repo =unitOfWork.GetRepository<PatientRelative,int>();
            await repo.AddAsync(new PatientRelative { RelativeId = relativeId, PatientId = patientId });
            await unitOfWork.CompleteAsync();
        }
        public async Task<RelativePatientsDto> GetRelativePatients(int relativeId)
        {
            var spec = new GetRelativePatientsSpecification(relativeId);
            var relative = await unitOfWork.GetRepository<Relative, int>().GetWithSpecAsync(spec);
            var relativePatientsDto = new RelativePatientsDto
            {
                RelativeSummary =
                    mapper.Map<RelativeSummaryDto>(relative),

                PatientsSummary =
                    mapper.Map<IEnumerable<PatientSummaryDto>>(
                    relative?.Patients)
            };
            return relativePatientsDto;
        }
        public async Task<RelativePatientHomeDto> GetPatientHomeAsync(int relativeId, int patientId)
        {
            var sepcs = new PatientRelativeHomeSpecifications(relativeId, patientId);
            var relativePatient = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(sepcs);
            var relativePatientsDto = new RelativePatientHomeDto
            {
                RelativeSummary =
                  mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                Patient =
                  mapper.Map<PatientHomeDto>(relativePatient?.Patient)

            };
            return relativePatientsDto;
        }
        public async Task<RelativePatientRemindersDto> GetPatientReminders(int relativeId, int patientId, int reminderType)
        {
            var specs = new RelativePatientRemindersSpecifications(relativeId, patientId);
            var relativePatient = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(specs);
            if (relativePatient is null)
                throw new NotFoundException(nameof(relativePatient), new { rId = relativeId, pId = patientId });
            RelativePatientRemindersDto patientsRemindesDto;

            switch (reminderType)
            {
                case 1:
                    patientsRemindesDto = new RelativePatientRemindersDto
                    {

                        RelativeSummary =
                         mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                        PatientMedications =
                         mapper.Map<PatientMedicationsDto>(relativePatient?.Patient),

                        PatientAppointments = null,
                        PatientRoutines = null

                    }; ;


                    break;


                case 2:

                    patientsRemindesDto = new RelativePatientRemindersDto
                    {

                        RelativeSummary =
                         mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                        PatientMedications = null,

                        PatientAppointments =
                         mapper.Map<PatientAppointmentsDto>(relativePatient?.Patient),

                        PatientRoutines = null

                    }; ;

                    break;
                case 3:
                    patientsRemindesDto = new RelativePatientRemindersDto
                    {

                        RelativeSummary =
                         mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                        PatientMedications = null,

                        PatientAppointments = null,

                        PatientRoutines = mapper.Map<PatientRoutineDto>(relativePatient?.Patient)

                    }; ;

                    break;


                default:
                    throw new Exception("Invalid reminder type");
            }
            return patientsRemindesDto;
        }
        public async Task CreateReminder(int relativeId, int patientId, ReminderToCreateDto dto)
        {
            var spec = new RelativePatientCheckSpecifications(relativeId, patientId);
            var relation = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(spec);

            if (relation is null)
                throw new NotFoundException(nameof(PatientRelative), new { relativeId, patientId });

            var reminder =
                 mapper.Map<Medication>(dto);

            reminder.PatientId = patientId;

            await unitOfWork.GetRepository<Medication, int>().AddAsync(reminder);

            await unitOfWork.CompleteAsync();
        }
        public async Task UpdateReminder(int relativeId,int patientId,int reminderId,ReminderToUpdateDto dto)
        {
            var relationSpec = new RelativePatientCheckSpecifications(relativeId,patientId);

            var relation =await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(relationSpec);

            if (relation is null)
                throw new NotFoundException( nameof(PatientRelative),new{relativeId,patientId});

            var reminderRepo =unitOfWork.GetRepository<Medication, int>();

            var reminder =await reminderRepo.Get(reminderId);

            if (reminder?.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });

            mapper.Map(dto, reminder);

            reminderRepo.Update(reminder);

            await unitOfWork.CompleteAsync();
        }
        public async Task DeleteReminder(int relativeId,int patientId,int reminderId)
        {
            var relationSpec =new RelativePatientCheckSpecifications(relativeId,patientId);

            var relation =await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(relationSpec);

            if (relation is null)
                  throw new NotFoundException( nameof(PatientRelative),new { relativeId, patientId });

            var reminderRepo = unitOfWork.GetRepository<Medication, int>();

            var reminder =await reminderRepo.Get(reminderId);

            if (reminder?.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });


            reminderRepo.Delete(reminder);

            await unitOfWork.CompleteAsync();
        }
    }

}

